using Constriva.Domain.Entities.Estoque;

namespace Constriva.Domain.Interfaces.Repositories;

public interface IMaterialRepository : ITenantRepository<Material>
{
    Task<Material?> GetByCodigoAsync(Guid empresaId, string codigo, CancellationToken ct = default);
    Task<IEnumerable<Material>> SearchAsync(Guid empresaId, string termo, CancellationToken ct = default);
    Task<Material?> GetWithGrupoAsync(Guid id, Guid empresaId, CancellationToken ct = default);
    Task<IEnumerable<Material>> GetAllComGrupoAsync(Guid empresaId, CancellationToken ct = default);
}
