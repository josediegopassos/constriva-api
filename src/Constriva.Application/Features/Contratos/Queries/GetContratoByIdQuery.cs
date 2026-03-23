using MediatR;
using Constriva.Application.Common.Behaviors;
using Constriva.Domain.Interfaces.Repositories;
using Constriva.Application.Features.Contratos.DTOs;

namespace Constriva.Application.Features.Contratos.Queries;

public record GetContratoByIdQuery(Guid Id, Guid EmpresaId) : IRequest<ContratoDetalheDto?>, ITenantRequest;

public class GetContratoByIdHandler : IRequestHandler<GetContratoByIdQuery, ContratoDetalheDto?>
{
    private readonly IContratoRepository _repo;
    public GetContratoByIdHandler(IContratoRepository repo) => _repo = repo;

    public async Task<ContratoDetalheDto?> Handle(GetContratoByIdQuery r, CancellationToken ct)
    {
        var c = await _repo.GetByIdComDetalhesAsync(r.Id, r.EmpresaId, ct);
        if (c == null || c.IsDeleted) return null;

        return new ContratoDetalheDto(
            c.Id, c.Numero, c.Objeto, c.Descricao,
            c.Tipo, c.Status,
            c.ObraId, c.FornecedorId, c.Fornecedor?.RazaoSocial,
            c.ValorGlobal, c.ValorAditivos, c.ValorTotal,
            c.ValorMedidoAcumulado, c.ValorPagoAcumulado,
            c.PercentualRetencao, c.ValorRetencao,
            c.CondicoesPagamento, c.DiasParaMedicao, c.DiasParaPagamento,
            c.DataAssinatura, c.DataVigenciaInicio, c.DataVigenciaFim,
            c.DataEncerramento,
            c.ArquivoUrl, c.AssinadoPor, c.FiscalId,
            c.Observacoes, c.CreatedAt);
    }
}
