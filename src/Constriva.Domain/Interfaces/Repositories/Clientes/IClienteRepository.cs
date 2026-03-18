using Constriva.Domain.Entities.Clientes;
using Constriva.Domain.Enums;

namespace Constriva.Domain.Interfaces.Repositories;

public interface IClienteRepository : ITenantRepository<Cliente>
{
    Task<(IEnumerable<Cliente> Items, int Total)> GetPagedAsync(
        Guid empresaId, string? search, StatusClienteEnum? status, int page, int pageSize, CancellationToken ct = default);
    Task<int> GetCountByEmpresaAsync(Guid empresaId, CancellationToken ct = default);
    Task<bool> DocumentoExistsAsync(string documento, Guid empresaId, Guid? excludeId, CancellationToken ct = default);
}
