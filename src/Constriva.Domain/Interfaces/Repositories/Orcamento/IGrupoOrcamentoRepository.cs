using Constriva.Domain.Entities.Orcamento;

namespace Constriva.Domain.Interfaces.Repositories;

public interface IGrupoOrcamentoRepository
{
    Task<GrupoOrcamento?> GetByIdAsync(Guid id, Guid empresaId, CancellationToken ct = default);
    Task<GrupoOrcamento?> GetWithItensAsync(Guid id, Guid empresaId, CancellationToken ct = default);
    Task<IEnumerable<GrupoOrcamento>> GetByOrcamentoAsync(Guid orcamentoId, CancellationToken ct = default);
    Task AddAsync(GrupoOrcamento grupo, CancellationToken ct = default);
    void Update(GrupoOrcamento grupo);
    void Remove(GrupoOrcamento grupo);
    Task<int> GetMaxOrdemAsync(Guid orcamentoId, CancellationToken ct = default);
    Task RecalcularTotaisAsync(Guid orcamentoId, Guid empresaId, CancellationToken ct = default);
}
