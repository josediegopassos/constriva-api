using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Constriva.Domain.Entities.Common;
using Constriva.Domain.Entities.Tenant;
using Constriva.Domain.Entities.Obras;
using Constriva.Domain.Entities.Estoque;
using Constriva.Domain.Entities.Compras;
using Constriva.Domain.Entities.Financeiro;
using Constriva.Domain.Entities.Cronograma;
using Constriva.Domain.Entities.Orcamento;
using Constriva.Domain.Entities.Contratos;
using Constriva.Domain.Entities.RH;
using Constriva.Domain.Entities.Qualidade;
using Constriva.Domain.Entities.SST;
using Constriva.Domain.Entities.GED;
using Constriva.Domain.Enums;
using Constriva.Domain.Interfaces.Repositories;
using System.Linq.Expressions;

namespace Constriva.Infrastructure.Persistence.Repositories;

// ─── Empresa Repository ──────────────────────────────────────────────────────
public class EmpresaRepository : Repository<Empresa>, IEmpresaRepository
{
    public EmpresaRepository(AppDbContext ctx) : base(ctx) { }

    public async Task<Empresa?> GetByCnpjAsync(string cnpj, CancellationToken ct = default)
        => await _set.FirstOrDefaultAsync(e => e.Cnpj == cnpj, ct);

    public async Task<bool> ExistsByNomeAsync(string razaoSocial, string nomeFantasia, CancellationToken ct = default)
        => await _set.AnyAsync(e => !e.IsDeleted &&
            (e.RazaoSocial == razaoSocial.Trim() || e.NomeFantasia == nomeFantasia.Trim()), ct);

    public async Task<IEnumerable<Empresa>> GetActivasAsync(CancellationToken ct = default)
        => await _set.Where(e => e.Status == StatusEmpresaEnum.Ativa && !e.IsDeleted).ToListAsync(ct);

    public async Task<Empresa?> GetWithUsuariosAsync(Guid id, CancellationToken ct = default)
        => await _set.Include(e => e.Usuarios).FirstOrDefaultAsync(e => e.Id == id, ct);

    public async Task<IEnumerable<Empresa>> GetVencimentoProximoAsync(int dias, CancellationToken ct = default)
    {
        var limite = DateTime.UtcNow.AddDays(dias);
        return await _set.Where(e => e.Status == StatusEmpresaEnum.Ativa && e.DataVencimento.HasValue && e.DataVencimento <= limite).ToListAsync(ct);
    }

    public async Task<(IEnumerable<Empresa> Items, int Total)> GetPagedAsync(
        string? search, StatusEmpresaEnum? status, int page, int pageSize, CancellationToken ct = default)
    {
        var q = _set.Where(e => !e.IsDeleted);
        if (!string.IsNullOrWhiteSpace(search))
            q = q.Where(e => e.RazaoSocial.Contains(search) || e.NomeFantasia.Contains(search) || e.Cnpj.Contains(search));
        if (status.HasValue) q = q.Where(e => e.Status == status.Value);
        var total = await q.CountAsync(ct);
        var items = await q.OrderByDescending(e => e.CreatedAt).Skip((page - 1) * pageSize).Take(pageSize).ToListAsync(ct);
        return (items, total);
    }
}

// ─── Usuario Repository ──────────────────────────────────────────────────────
public class UsuarioRepository : Repository<Usuario>, IUsuarioRepository
{
    public UsuarioRepository(AppDbContext ctx) : base(ctx) { }

    public async Task<Usuario?> GetByEmailAsync(string email, CancellationToken ct = default)
        => await _set.FirstOrDefaultAsync(u => u.Email == email.ToLower() && u.IsSuperAdmin, ct);

    public async Task<Usuario?> GetByEmailAndEmpresaAsync(string email, Guid empresaId, CancellationToken ct = default)
        => await _set.FirstOrDefaultAsync(u => u.Email == email.ToLower() && u.EmpresaId == empresaId, ct);

    public async Task<IEnumerable<Usuario>> GetByEmpresaAsync(Guid empresaId, CancellationToken ct = default)
        => await _set.Where(u => u.EmpresaId == empresaId && !u.IsDeleted).ToListAsync(ct);

    public async Task<Usuario?> GetByRefreshTokenAsync(string refreshToken, CancellationToken ct = default)
        => await _set.FirstOrDefaultAsync(u => u.RefreshToken == refreshToken, ct);

    public async Task<IEnumerable<UsuarioPermissao>> GetPermissoesAsync(Guid usuarioId, CancellationToken ct = default)
        => await _ctx.UsuariosPermissoes.Where(p => p.UsuarioId == usuarioId).ToListAsync(ct);

    public async Task<bool> EmailExistsAsync(string email, Guid? excludeId = null, CancellationToken ct = default)
    {
        var q = _set.Where(u => u.Email == email.ToLower());
        if (excludeId.HasValue) q = q.Where(u => u.Id != excludeId.Value);
        return await q.AnyAsync(ct);
    }

    public async Task<Usuario?> GetWithPermissoesAsync(Guid id, CancellationToken ct = default)
        => await _set.Include(u => u.Permissoes).FirstOrDefaultAsync(u => u.Id == id, ct);

    public async Task<(IEnumerable<Usuario> Items, int Total)> GetPagedAsync(
        Guid? empresaId, string? search, bool? ativo, int page, int pageSize, CancellationToken ct = default)
    {
        var q = _set.Where(u => !u.IsDeleted);
        if (empresaId.HasValue) q = q.Where(u => u.EmpresaId == empresaId);
        if (!string.IsNullOrWhiteSpace(search))
            q = q.Where(u => u.Nome.Contains(search) || u.Email.Contains(search));
        if (ativo.HasValue) q = q.Where(u => u.Ativo == ativo);
        var total = await q.CountAsync(ct);
        var items = await q.OrderBy(u => u.Nome).Skip((page - 1) * pageSize).Take(pageSize).ToListAsync(ct);
        return (items, total);
    }

    public async Task ReplacePermissoesAsync(Guid usuarioId, Guid empresaId,
        IEnumerable<UsuarioPermissao> permissoes, CancellationToken ct = default)
    {
        var existentes = await _ctx.UsuariosPermissoes
            .Where(p => p.UsuarioId == usuarioId && p.EmpresaId == empresaId).ToListAsync(ct);
        _ctx.UsuariosPermissoes.RemoveRange(existentes);
        await _ctx.UsuariosPermissoes.AddRangeAsync(permissoes, ct);
    }
}
