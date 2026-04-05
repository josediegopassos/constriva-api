using MediatR;
using Constriva.Application.Common.Behaviors;
using Constriva.Application.Features.Lens.DTOs;
using Constriva.Application.Features.Lens.Extensions;
using Constriva.Domain.Entities.Lens;
using Constriva.Domain.Enums;
using Constriva.Domain.Interfaces.Repositories;

namespace Constriva.Application.Features.Lens.Queries;

public record GetProcessamentoByIdQuery(
    Guid Id,
    Guid EmpresaId)
    : IRequest<ProcessamentoLensDetalheDto?>, ITenantRequest;

public class GetProcessamentoByIdHandler : IRequestHandler<GetProcessamentoByIdQuery, ProcessamentoLensDetalheDto?>
{
    private readonly IDocumentoLensRepository _repo;
    private readonly IRepository<Domain.Entities.Tenant.Usuario> _usuarioRepo;

    public GetProcessamentoByIdHandler(
        IDocumentoLensRepository repo,
        IRepository<Domain.Entities.Tenant.Usuario> usuarioRepo)
    {
        _repo = repo;
        _usuarioRepo = usuarioRepo;
    }

    public async Task<ProcessamentoLensDetalheDto?> Handle(GetProcessamentoByIdQuery r, CancellationToken ct)
    {
        var doc = await _repo.GetByIdWithItensAsync(r.Id, r.EmpresaId, ct);

        if (doc is null) return null;

        var usuario = await _usuarioRepo.GetByIdAsync(doc.UsuarioId, ct);

        return new ProcessamentoLensDetalheDto(
            doc.Id,
            doc.EmpresaId,
            doc.ObraId,
            doc.CentroCustoId,
            doc.FornecedorId,
            doc.UsuarioId,
            doc.TipoDocumento,
            doc.TipoDocumento.ToDescricao(),
            doc.TipoDocumentoDeclarado,
            doc.TiposConferem,
            doc.Status,
            doc.Status.ToString(),
            doc.NomeArquivo,
            doc.ExtensaoArquivo,
            doc.TamanhoBytes,
            doc.ConfidenceScore,
            doc.Warnings,
            doc.MensagemErro,
            doc.CodigoErro,
            doc.PodeReprocessar,
            doc.TentativaNumero,
            doc.TempoProcessamentoMs,
            doc.PaginasProcessadas,
            doc.Observacoes,
            doc.NumeroDocumentoExtraido,
            doc.DataEmissaoExtraida,
            doc.ValorTotalExtraido,
            doc.CnpjFornecedorExtraido,
            doc.NomeFornecedorExtraido,
            doc.CompraId,
            doc.Itens.OrderBy(i => i.OrdemItem).Select(MapItem).ToList(),
            doc.CreatedAt,
            doc.UpdatedAt,
            usuario?.Nome ?? "Usuário");
    }

    private static ItemDocumentoLensDto MapItem(ItemDocumentoLens i) => new(
        i.Id,
        i.OrdemItem,
        i.Codigo,
        i.Descricao,
        i.Ncm,
        i.Unidade,
        i.Quantidade,
        i.PrecoUnitario,
        i.PrecoTotal,
        i.Desconto,
        i.Status,
        i.Status.ToString(),
        i.MotivoRejeicao,
        i.EditadoPorUsuarioId.HasValue,
        i.DescricaoOriginalOcr,
        i.QuantidadeOriginalOcr,
        i.PrecoUnitarioOriginalOcr,
        null);
}
