using MediatR;
using Constriva.Application.Common.Behaviors;
using Constriva.Domain.Entities.Lens;
using Constriva.Domain.Enums;
using Constriva.Domain.Interfaces.Repositories;

namespace Constriva.Application.Features.Lens.Commands;

public record ApproveItemLensCommand(
    Guid ProcessamentoId,
    Guid ItemId,
    Guid EmpresaId,
    Guid UsuarioId) : IRequest, ITenantRequest;

public class ApproveItemLensCommandHandler : IRequestHandler<ApproveItemLensCommand>
{
    private readonly ITenantRepository<DocumentoLens> _docRepo;
    private readonly IRepository<ItemDocumentoLens> _itemRepo;
    private readonly IUnitOfWork _uow;

    public ApproveItemLensCommandHandler(
        ITenantRepository<DocumentoLens> docRepo,
        IRepository<ItemDocumentoLens> itemRepo,
        IUnitOfWork uow)
    {
        _docRepo = docRepo;
        _itemRepo = itemRepo;
        _uow = uow;
    }

    public async Task Handle(ApproveItemLensCommand r, CancellationToken ct)
    {
        var documento = await _docRepo.GetByIdAndEmpresaAsync(r.ProcessamentoId, r.EmpresaId, ct)
            ?? throw new KeyNotFoundException($"Documento Lens {r.ProcessamentoId} não encontrado.");

        var item = await _itemRepo.GetByIdAsync(r.ItemId, ct)
            ?? throw new KeyNotFoundException($"Item {r.ItemId} não encontrado.");

        if (item.DocumentoLensId != r.ProcessamentoId)
            throw new InvalidOperationException("O item não pertence ao processamento informado.");

        if (item.Status != StatusItemLensEnum.Pendente && item.Status != StatusItemLensEnum.Editado)
            throw new InvalidOperationException(
                $"Item com status '{item.Status}' não pode ser aprovado. Apenas itens 'Pendente' ou 'Editado' são permitidos.");

        item.Status = StatusItemLensEnum.Aprovado;
        item.UpdatedAt = DateTime.UtcNow;

        if (documento.Status == StatusProcessamentoLensEnum.AguardandoRevisao)
        {
            documento.Status = StatusProcessamentoLensEnum.EmRevisao;
            documento.UpdatedAt = DateTime.UtcNow;
            documento.UpdatedBy = r.UsuarioId;
            _docRepo.Update(documento);
        }

        _itemRepo.Update(item);
        await _uow.SaveChangesAsync(ct);
    }
}
