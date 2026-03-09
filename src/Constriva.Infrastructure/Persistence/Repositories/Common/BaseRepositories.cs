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

// ─── Generic Repository ──────────────────────────────────────────────────────
public class Repository<T> : IRepository<T> where T : BaseEntity
{
    protected readonly AppDbContext _ctx;
    protected readonly DbSet<T> _set;

    public Repository(AppDbContext ctx) { _ctx = ctx; _set = ctx.Set<T>(); }

    public async Task<T?> GetByIdAsync(Guid id, CancellationToken ct = default)
        => await _set.FirstOrDefaultAsync(e => e.Id == id, ct);

    public async Task<IEnumerable<T>> GetAllAsync(CancellationToken ct = default)
        => await _set.ToListAsync(ct);

    public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate, CancellationToken ct = default)
        => await _set.Where(predicate).ToListAsync(ct);

    public async Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate, CancellationToken ct = default)
        => await _set.FirstOrDefaultAsync(predicate, ct);

    public async Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate, CancellationToken ct = default)
        => await _set.AnyAsync(predicate, ct);

    public async Task<int> CountAsync(Expression<Func<T, bool>>? predicate = null, CancellationToken ct = default)
        => predicate == null ? await _set.CountAsync(ct) : await _set.CountAsync(predicate, ct);

    public async Task AddAsync(T entity, CancellationToken ct = default) => await _set.AddAsync(entity, ct);
    public async Task AddRangeAsync(IEnumerable<T> entities, CancellationToken ct = default) => await _set.AddRangeAsync(entities, ct);
    public void Update(T entity) => _set.Update(entity);
    public void Remove(T entity) => _set.Remove(entity);
    public void RemoveRange(IEnumerable<T> entities) => _set.RemoveRange(entities);
    public IQueryable<T> Query() => _set.AsQueryable();
}

// ─── Tenant Repository ───────────────────────────────────────────────────────
public class TenantRepository<T> : Repository<T>, ITenantRepository<T> where T : TenantEntity
{
    public TenantRepository(AppDbContext ctx) : base(ctx) { }

    public async Task<T?> GetByIdAndEmpresaAsync(Guid id, Guid empresaId, CancellationToken ct = default)
        => await _set.FirstOrDefaultAsync(e => e.Id == id && e.EmpresaId == empresaId, ct);

    public async Task<IEnumerable<T>> GetAllByEmpresaAsync(Guid empresaId, CancellationToken ct = default)
        => await _set.Where(e => e.EmpresaId == empresaId).ToListAsync(ct);

    public IQueryable<T> QueryByEmpresa(Guid empresaId)
        => _set.Where(e => e.EmpresaId == empresaId);
}

// ─── Unit of Work ────────────────────────────────────────────────────────────
public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _ctx;
    private IDbContextTransaction? _transaction;

    public UnitOfWork(AppDbContext ctx) => _ctx = ctx;

    public async Task<int> SaveChangesAsync(CancellationToken ct = default) => await _ctx.SaveChangesAsync(ct);
    public async Task BeginTransactionAsync(CancellationToken ct = default) => _transaction = await _ctx.Database.BeginTransactionAsync(ct);
    public async Task CommitTransactionAsync(CancellationToken ct = default) { await _ctx.SaveChangesAsync(ct); await _transaction!.CommitAsync(ct); }
    public async Task RollbackTransactionAsync(CancellationToken ct = default) => await _transaction!.RollbackAsync(ct);
    public void Dispose() { _transaction?.Dispose(); _ctx.Dispose(); }
}
