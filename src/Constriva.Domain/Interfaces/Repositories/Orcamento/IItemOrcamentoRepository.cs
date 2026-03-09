using Constriva.Domain.Entities.Orcamento;

namespace Constriva.Domain.Interfaces.Repositories;

public interface IItemOrcamentoRepository
{
    Task<ItemOrcamento?> GetByIdAsync(Guid id, Guid empresaId, CancellationToken ct = default);
    Task<IEnumerable<ItemOrcamento>> GetByGrupoAsync(Guid grupoId, CancellationToken ct = default);
    Task<IEnumerable<ItemOrcamento>> GetByOrcamentoAsync(Guid orcamentoId, CancellationToken ct = default);
    Task AddAsync(ItemOrcamento item, CancellationToken ct = default);
    void Update(ItemOrcamento item);
    void Remove(ItemOrcamento item);
    Task<int> GetMaxOrdemAsync(Guid grupoId, CancellationToken ct = default);
}
