using Constriva.Domain.Entities.Fornecedores;
using Constriva.Domain.Enums;

namespace Constriva.Domain.Interfaces.Repositories;

public interface IFornecedorRepository : ITenantRepository<Fornecedor>
{
    Task<Fornecedor?> GetByIdComEnderecoAsync(Guid id, Guid empresaId, CancellationToken ct = default);
    Task<Fornecedor?> GetByDocumentoAsync(Guid empresaId, string documento, CancellationToken ct = default);
    Task<IEnumerable<Fornecedor>> SearchAsync(Guid empresaId, string termo, CancellationToken ct = default);
    Task<(IEnumerable<Fornecedor> Items, int Total)> GetPagedAsync(
        Guid empresaId, string? search, TipoFornecedorEnum? tipo, int page, int pageSize, CancellationToken ct = default);
}
