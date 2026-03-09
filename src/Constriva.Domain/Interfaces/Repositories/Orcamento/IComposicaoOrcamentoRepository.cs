using Constriva.Domain.Entities.Orcamento;

namespace Constriva.Domain.Interfaces.Repositories;

public interface IComposicaoOrcamentoRepository
{
    Task<IEnumerable<ComposicaoOrcamento>> GetByOrcamentoAsync(Guid orcamentoId, Guid empresaId, CancellationToken ct = default);
    Task<ComposicaoOrcamento?> GetByIdAsync(Guid id, Guid empresaId, CancellationToken ct = default);
    Task<ComposicaoOrcamento?> GetWithInsumosAsync(Guid id, Guid empresaId, CancellationToken ct = default);
    Task AddAsync(ComposicaoOrcamento composicao, CancellationToken ct = default);
    Task AddInsumoAsync(InsumoComposicao insumo, CancellationToken ct = default);
    Task<bool> CodigoExisteAsync(Guid orcamentoId, string codigo, Guid? excludeId, CancellationToken ct = default);
    void Update(ComposicaoOrcamento composicao);
    void Remove(ComposicaoOrcamento composicao);
}
