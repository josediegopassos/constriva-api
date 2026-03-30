using Constriva.Domain.Entities.Clientes;
using Constriva.Domain.Entities.Common;
using Constriva.Domain.Enums;

namespace Constriva.Domain.Interfaces.Repositories;

public interface IClienteRepository : ITenantRepository<Cliente>
{
    Task<Cliente?> GetByIdComEnderecoAsync(Guid id, Guid empresaId, CancellationToken ct = default);
    Task<(IEnumerable<Cliente> Items, int Total)> GetPagedAsync(
        Guid empresaId, string? search, StatusClienteEnum? status, int page, int pageSize, CancellationToken ct = default);
    Task<int> GetCountByEmpresaAsync(Guid empresaId, CancellationToken ct = default);
    Task<bool> DocumentoExistsAsync(string documento, Guid empresaId, Guid? excludeId, CancellationToken ct = default);
    Task<IEnumerable<Cliente>> GetAllActiveByEmpresaAsync(Guid empresaId, CancellationToken ct = default);
    Task AddEnderecoAsync(Endereco endereco, CancellationToken ct = default);
}
