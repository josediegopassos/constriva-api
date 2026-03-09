using Constriva.Domain.Entities.Tenant;

namespace Constriva.Domain.Interfaces.Repositories;

public interface IUsuarioRepository : IRepository<Usuario>
{
    Task<Usuario?> GetByEmailAsync(string email, CancellationToken ct = default);
    Task<Usuario?> GetByEmailAndEmpresaAsync(string email, Guid empresaId, CancellationToken ct = default);
    Task<IEnumerable<Usuario>> GetByEmpresaAsync(Guid empresaId, CancellationToken ct = default);
    Task<Usuario?> GetByRefreshTokenAsync(string refreshToken, CancellationToken ct = default);
    Task<IEnumerable<UsuarioPermissao>> GetPermissoesAsync(Guid usuarioId, CancellationToken ct = default);
    Task<bool> EmailExistsAsync(string email, Guid? excludeId = null, CancellationToken ct = default);
    Task<Usuario?> GetWithPermissoesAsync(Guid id, CancellationToken ct = default);
    Task<(IEnumerable<Usuario> Items, int Total)> GetPagedAsync(
        Guid? empresaId, string? search, bool? ativo, int page, int pageSize, CancellationToken ct = default);
    Task ReplacePermissoesAsync(Guid usuarioId, Guid empresaId,
        IEnumerable<UsuarioPermissao> permissoes, CancellationToken ct = default);
}
