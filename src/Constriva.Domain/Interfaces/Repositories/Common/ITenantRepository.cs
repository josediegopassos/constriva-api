using Constriva.Domain.Entities.Common;

namespace Constriva.Domain.Interfaces.Repositories;

public interface ITenantRepository<T> : IRepository<T> where T : TenantEntity
{
    Task<T?> GetByIdAndEmpresaAsync(Guid id, Guid empresaId, CancellationToken ct = default);
    Task<IEnumerable<T>> GetAllByEmpresaAsync(Guid empresaId, CancellationToken ct = default);
    IQueryable<T> QueryByEmpresa(Guid empresaId);
}
