using Constriva.Domain.Entities.Orcamento;

namespace Constriva.Domain.Interfaces.Repositories;

public interface IOrcamentoRepository
{
    Task<IEnumerable<Orcamento>> GetByEmpresaAsync(Guid empresaId, Guid? obraId, int page, int pageSize, CancellationToken ct = default);
    Task<int> CountByEmpresaAsync(Guid empresaId, Guid? obraId, CancellationToken ct = default);
    Task<IEnumerable<Orcamento>> GetByObraAsync(Guid obraId, Guid empresaId, CancellationToken ct = default);
    Task<Orcamento?> GetByIdAsync(Guid id, Guid empresaId, CancellationToken ct = default);
    Task<Orcamento?> GetWithGruposItensAsync(Guid id, Guid empresaId, CancellationToken ct = default);
    Task AddAsync(Orcamento orcamento, CancellationToken ct = default);
    void Update(Orcamento orcamento);
    void Remove(Orcamento orcamento);
    Task<int> GetMaxVersaoAsync(Guid obraId, Guid empresaId, CancellationToken ct = default);
    Task<bool> ExistsLinhaDBaseAsync(Guid obraId, Guid empresaId, CancellationToken ct = default);
    Task AddHistoricoAsync(OrcamentoHistorico historico, CancellationToken ct = default);
    Task<IEnumerable<OrcamentoHistorico>> GetHistoricoAsync(Guid orcamentoId, Guid empresaId, CancellationToken ct = default);
    Task<OrcamentoDashboardStats> GetDashboardStatsAsync(Guid empresaId, CancellationToken ct = default);
}

public record OrcamentoDashboardStats(int Total, int Rascunhos, int EmRevisao, int Aprovados, decimal ValorTotalAprovado);
