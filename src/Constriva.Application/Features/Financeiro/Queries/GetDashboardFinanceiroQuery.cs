using MediatR;
using Constriva.Application.Common.Behaviors;
using Constriva.Application.Common.Interfaces;
using Constriva.Application.Features.Financeiro;
using Constriva.Domain.Enums;
using Constriva.Domain.Interfaces.Repositories;
using Constriva.Application.Features.Financeiro.DTOs;

namespace Constriva.Application.Features.Financeiro.Queries;

public record GetDashboardFinanceiroQuery(Guid EmpresaId, Guid? ObraId)
    : IRequest<DashboardFinanceiroDto>, ITenantRequest { public Guid TenantId => EmpresaId; }

public class GetDashboardFinanceiroHandler : IRequestHandler<GetDashboardFinanceiroQuery, DashboardFinanceiroDto>
{
    private readonly ILancamentoFinanceiroRepository _repo;
    public GetDashboardFinanceiroHandler(ILancamentoFinanceiroRepository repo) { _repo = repo; }

    public async Task<DashboardFinanceiroDto> Handle(GetDashboardFinanceiroQuery r, CancellationToken ct)
    {
        var stats = await _repo.GetDashboardStatsAsync(r.EmpresaId, r.ObraId, ct);
        return new DashboardFinanceiroDto(
            stats.TotalReceitas, stats.TotalDespesas,
            stats.TotalReceitas - stats.TotalDespesas,
            stats.ReceitasRealizadas, stats.DespesasRealizadas,
            stats.LancamentosVencidos, stats.LancamentosVencendo);
    }
}

