using Constriva.Domain.Entities.Financeiro;
using Constriva.Domain.Enums;

namespace Constriva.Domain.Interfaces.Repositories;

public interface ILancamentoFinanceiroRepository : ITenantRepository<LancamentoFinanceiro>
{
    Task<(IEnumerable<LancamentoFinanceiro> Items, int Total)> GetPagedAsync(
        Guid empresaId, Guid? obraId, DateTime? inicio, DateTime? fim,
        TipoLancamentoEnum? tipo, StatusLancamentoEnum? status, int page, int pageSize, CancellationToken ct = default);
    Task<IEnumerable<LancamentoFinanceiro>> GetByObraAsync(Guid empresaId, Guid obraId, CancellationToken ct = default);
    Task<IEnumerable<LancamentoFinanceiro>> GetVencidosAsync(Guid empresaId, CancellationToken ct = default);
    Task<IEnumerable<LancamentoFinanceiro>> GetByPeriodoAsync(Guid empresaId, DateTime inicio, DateTime fim, CancellationToken ct = default);
    Task<FinanceiroDashboardStats> GetDashboardStatsAsync(Guid empresaId, Guid? obraId, CancellationToken ct = default);
    Task<IEnumerable<FluxoMensalData>> GetFluxoMensalAsync(Guid empresaId, Guid? obraId, int meses, CancellationToken ct = default);
}

public record FinanceiroDashboardStats(
    decimal TotalReceitas, decimal TotalDespesas,
    decimal ReceitasRealizadas, decimal DespesasRealizadas,
    int LancamentosVencidos, int LancamentosVencendo
);

public record FluxoMensalData(int Ano, int Mes, decimal Receitas, decimal Despesas);
