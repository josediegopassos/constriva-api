using MediatR;
using Constriva.Application.Common.Behaviors;
using Constriva.Application.Features.Lens.DTOs;
using Constriva.Domain.Entities.Lens;
using Constriva.Domain.Enums;
using Constriva.Domain.Interfaces.Repositories;

namespace Constriva.Application.Features.Lens.Commands;

public record EditItemLensCommand(
    Guid ProcessamentoId,
    Guid ItemId,
    Guid EmpresaId,
    Guid UsuarioId,
    EditItemLensDto Dto) : IRequest, ITenantRequest;

public class EditItemLensCommandHandler : IRequestHandler<EditItemLensCommand>
{
    private readonly ITenantRepository<DocumentoLens> _docRepo;
    private readonly IRepository<ItemDocumentoLens> _itemRepo;
    private readonly IUnitOfWork _uow;

    public EditItemLensCommandHandler(
        ITenantRepository<DocumentoLens> docRepo,
        IRepository<ItemDocumentoLens> itemRepo,
        IUnitOfWork uow)
    {
        _docRepo = docRepo;
        _itemRepo = itemRepo;
        _uow = uow;
    }

    public async Task Handle(EditItemLensCommand r, CancellationToken ct)
    {
        var documento = await _docRepo.GetByIdAndEmpresaAsync(r.ProcessamentoId, r.EmpresaId, ct)
            ?? throw new KeyNotFoundException($"Documento Lens {r.ProcessamentoId} não encontrado.");

        var item = await _itemRepo.GetByIdAsync(r.ItemId, ct)
            ?? throw new KeyNotFoundException($"Item {r.ItemId} não encontrado.");

        if (item.DocumentoLensId != r.ProcessamentoId)
            throw new InvalidOperationException("O item não pertence ao processamento informado.");

        // Salvar valores originais do OCR na primeira edição
        if (item.DescricaoOriginalOcr is null)
        {
            item.DescricaoOriginalOcr = item.Descricao;
            item.QuantidadeOriginalOcr = item.Quantidade;
            item.PrecoUnitarioOriginalOcr = item.PrecoUnitario;
            item.PrecoTotalOriginalOcr = item.PrecoTotal;
        }

        // Aplicar alterações
        if (r.Dto.Descricao is not null) item.Descricao = r.Dto.Descricao;
        if (r.Dto.Quantidade.HasValue) item.Quantidade = r.Dto.Quantidade.Value;
        if (r.Dto.PrecoUnitario.HasValue) item.PrecoUnitario = r.Dto.PrecoUnitario.Value;
        if (r.Dto.PrecoTotal.HasValue) item.PrecoTotal = r.Dto.PrecoTotal.Value;

        item.Status = StatusItemLensEnum.Editado;
        item.EditadoPorUsuarioId = r.UsuarioId;
        item.EditadoEm = DateTime.UtcNow;
        item.UpdatedAt = DateTime.UtcNow;

        _itemRepo.Update(item);
        await _uow.SaveChangesAsync(ct);
    }
}
