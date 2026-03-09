using MediatR;
using Constriva.Application.Common.Behaviors;
using Constriva.Application.Common.Interfaces;
using Constriva.Application.Features.Financeiro;
using Constriva.Domain.Enums;
using Constriva.Domain.Interfaces.Repositories;
using Constriva.Application.Features.Financeiro.DTOs;

namespace Constriva.Application.Features.Financeiro.Queries;

public record GetLancamentosQuery(
    Guid EmpresaId, Guid? ObraId, DateTime? Inicio, DateTime? Fim,
    TipoLancamentoEnum? Tipo, StatusLancamentoEnum? Status, int Page = 1, int PageSize = 20)
    : IRequest<PaginatedResult<LancamentoFinanceiroDto>>, ITenantRequest
{ public Guid TenantId => EmpresaId; }

public class GetLancamentosHandler : IRequestHandler<GetLancamentosQuery, PaginatedResult<LancamentoFinanceiroDto>>
{
    private readonly ILancamentoFinanceiroRepository _repo;
    public GetLancamentosHandler(ILancamentoFinanceiroRepository repo) { _repo = repo; }

    public async Task<PaginatedResult<LancamentoFinanceiroDto>> Handle(GetLancamentosQuery r, CancellationToken ct)
    {
        var (items, total) = await _repo.GetPagedAsync(
            r.EmpresaId, r.ObraId, r.Inicio, r.Fim, r.Tipo, r.Status, r.Page, r.PageSize, ct);

        var dtos = items.Select(l => new LancamentoFinanceiroDto(
            l.Id, l.Descricao, l.Tipo, l.Status, l.Valor, l.ValorRealizado,
            l.DataVencimento, l.DataPagamento, l.FormaPagamentoEnum,
            l.NumeroDocumento, l.NumeroNF, l.Observacoes,
            l.ObraId, null, l.FornecedorId, null,
            l.CentroCustoId, l.CentroCusto?.Nome, l.CreatedAt));

        return new PaginatedResult<LancamentoFinanceiroDto> { Items = dtos, TotalCount = total, Page = r.Page, PageSize = r.PageSize };
    }
}

