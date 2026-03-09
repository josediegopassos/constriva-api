using Constriva.Domain.Entities.Obras;
using Constriva.Domain.Enums;

namespace Constriva.Domain.Interfaces.Repositories;

public interface IObraRepository : ITenantRepository<Obra>
{
    Task<(IEnumerable<Obra> Items, int Total)> GetPagedAsync(
        Guid empresaId, string? search, StatusObraEnum? status, TipoObraEnum? tipo,
        int page, int pageSize, CancellationToken ct = default);
    Task<Obra?> GetWithFasesAsync(Guid id, Guid empresaId, CancellationToken ct = default);
    Task<IEnumerable<Obra>> GetByStatusAsync(Guid empresaId, StatusObraEnum status, CancellationToken ct = default);
    Task<Obra?> GetByCodigoAsync(Guid empresaId, string codigo, CancellationToken ct = default);
    Task<int> CountByEmpresaAsync(Guid empresaId, StatusObraEnum? status = null, CancellationToken ct = default);
    Task<ObrasDashboardStats> GetDashboardStatsAsync(Guid empresaId, CancellationToken ct = default);
    Task<IEnumerable<Obra>> GetUltimasObrasAsync(Guid empresaId, int count, CancellationToken ct = default);
    Task AddHistoricoAsync(ObraHistorico historico, CancellationToken ct = default);
}

public record ObrasDashboardStats(
    int Total, int EmAndamento, int Concluidas, int Paralisadas, int Atrasadas,
    decimal ValorTotalContratos, decimal ValorTotalRealizado, decimal PercentualMedio
);
