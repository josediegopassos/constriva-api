using Constriva.Domain.Entities.Obras;

namespace Constriva.Domain.Interfaces.Repositories;

public interface IFaseObraRepository : IRepository<FaseObra>
{
    Task<IEnumerable<FaseObra>> GetByObraAsync(Guid obraId, CancellationToken ct = default);
}
