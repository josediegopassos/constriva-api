using MediatR;
using Constriva.Application.Common.Behaviors;
using Constriva.Application.Common.Interfaces;
using Constriva.Application.Features.Financeiro;
using Constriva.Domain.Enums;
using Constriva.Domain.Interfaces.Repositories;
using Constriva.Application.Features.Financeiro.DTOs;

namespace Constriva.Application.Features.Financeiro.Queries;

public record GetFluxoCaixaQuery(Guid EmpresaId, Guid? ObraId, DateTime? Inicio, DateTime? Fim)
    : IRequest<IEnumerable<FluxoCaixaItemDto>>, ITenantRequest { public Guid TenantId => EmpresaId; }

public class GetFluxoCaixaHandler : IRequestHandler<GetFluxoCaixaQuery, IEnumerable<FluxoCaixaItemDto>>
{
    private readonly ILancamentoFinanceiroRepository _repo;
    public GetFluxoCaixaHandler(ILancamentoFinanceiroRepository repo) { _repo = repo; }

    public async Task<IEnumerable<FluxoCaixaItemDto>> Handle(GetFluxoCaixaQuery r, CancellationToken ct)
    {
        var dados = await _repo.GetFluxoMensalAsync(r.EmpresaId, r.ObraId, 12, ct);
        var acumulado = 0m;
        return dados.Select(d =>
        {
            acumulado += d.Receitas - d.Despesas;
            return new FluxoCaixaItemDto(d.Ano, d.Mes, d.Receitas, d.Despesas, acumulado);
        });
    }
}

