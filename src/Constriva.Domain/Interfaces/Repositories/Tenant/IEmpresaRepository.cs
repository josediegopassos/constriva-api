using Constriva.Domain.Entities.Tenant;
using Constriva.Domain.Enums;

namespace Constriva.Domain.Interfaces.Repositories;

public interface IEmpresaRepository : IRepository<Empresa>
{
    Task<Empresa?> GetByCnpjAsync(string cnpj, CancellationToken ct = default);
    Task<bool> ExistsByNomeAsync(string razaoSocial, string nomeFantasia, CancellationToken ct = default);
    Task<IEnumerable<Empresa>> GetActivasAsync(CancellationToken ct = default);
    Task<Empresa?> GetWithUsuariosAsync(Guid id, CancellationToken ct = default);
    Task<IEnumerable<Empresa>> GetVencimentoProximoAsync(int dias, CancellationToken ct = default);
    Task<(IEnumerable<Empresa> Items, int Total)> GetPagedAsync(
        string? search, StatusEmpresaEnum? status, int page, int pageSize, CancellationToken ct = default);
}
