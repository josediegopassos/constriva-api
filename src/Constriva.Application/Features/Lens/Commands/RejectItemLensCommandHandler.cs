using MediatR;
using Constriva.Application.Common.Behaviors;
using Constriva.Domain.Entities.Lens;
using Constriva.Domain.Enums;
using Constriva.Domain.Interfaces.Repositories;

namespace Constriva.Application.Features.Lens.Commands;

public record RejectItemLensCommand(
    Guid ProcessamentoId,
    Guid ItemId,
    Guid EmpresaId,
    Guid UsuarioId,
    string? Motivo) : IRequest, ITenantRequest;

public class RejectItemLensCommandHandler : IRequestHandler<RejectItemLensCommand>
{
    private readonly ITenantRepository<DocumentoLens> _docRepo;
    private readonly IRepository<ItemDocumentoLens> _itemRepo;
    private readonly IUnitOfWork _uow;

    public RejectItemLensCommandHandler(
        ITenantRepository<DocumentoLens> docRepo,
        IRepository<ItemDocumentoLens> itemRepo,
        IUnitOfWork uow)
    {
        _docRepo = docRepo;
        _itemRepo = itemRepo;
        _uow = uow;
    }

    public async Task Handle(RejectItemLensCommand r, CancellationToken ct)
    {
        var documento = await _docRepo.GetByIdAndEmpresaAsync(r.ProcessamentoId, r.EmpresaId, ct)
            ?? throw new KeyNotFoundException($"Documento Lens {r.ProcessamentoId} não encontrado.");

        var item = await _itemRepo.GetByIdAsync(r.ItemId, ct)
            ?? throw new KeyNotFoundException($"Item {r.ItemId} não encontrado.");

        if (item.DocumentoLensId != r.ProcessamentoId)
            throw new InvalidOperationException("O item não pertence ao processamento informado.");

        item.Status = StatusItemLensEnum.Rejeitado;
        item.MotivoRejeicao = r.Motivo;
        item.UpdatedAt = DateTime.UtcNow;

        _itemRepo.Update(item);
        await _uow.SaveChangesAsync(ct);
    }
}
