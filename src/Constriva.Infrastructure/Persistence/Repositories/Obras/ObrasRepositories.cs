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

// ─── Obra Repository ─────────────────────────────────────────────────────────
public class ObraRepository : TenantRepository<Obra>, IObraRepository
{
    public ObraRepository(AppDbContext ctx) : base(ctx) { }

    public async Task<Obra?> GetByIdComEnderecoAsync(Guid id, Guid empresaId, CancellationToken ct = default)
        => await _set.Include(o => o.Endereco)
            .FirstOrDefaultAsync(o => o.Id == id && o.EmpresaId == empresaId, ct);

    public async Task<IEnumerable<Obra>> GetByStatusAsync(Guid empresaId, StatusObraEnum status, CancellationToken ct = default)
        => await _set.Where(o => o.EmpresaId == empresaId && o.Status == status).ToListAsync(ct);

    public async Task<Obra?> GetByCodigoAsync(Guid empresaId, string codigo, CancellationToken ct = default)
        => await _set.FirstOrDefaultAsync(o => o.EmpresaId == empresaId && o.Codigo == codigo, ct);

    public async Task<Obra?> GetWithFasesAsync(Guid id, Guid empresaId, CancellationToken ct = default)
        => await _set.Include(o => o.Cliente).Include(o => o.Endereco).Include(o => o.Fases).ThenInclude(f => f.SubFases)
            .FirstOrDefaultAsync(o => o.Id == id && o.EmpresaId == empresaId, ct);

    public async Task<IEnumerable<Obra>> GetByUsuarioAsync(Guid usuarioId, Guid empresaId, CancellationToken ct = default)
    {
        var obraIds = await _ctx.UsuariosObras
            .Where(uo => uo.UsuarioId == usuarioId && uo.EmpresaId == empresaId && uo.Ativo)
            .Select(uo => uo.ObraId).ToListAsync(ct);
        return await _set.Where(o => obraIds.Contains(o.Id) && o.EmpresaId == empresaId).ToListAsync(ct);
    }

    public async Task<int> CountByEmpresaAsync(Guid empresaId, StatusObraEnum? status = null, CancellationToken ct = default)
    {
        var q = _set.Where(o => o.EmpresaId == empresaId && !o.IsDeleted);
        if (status.HasValue) q = q.Where(o => o.Status == status.Value);
        return await q.CountAsync(ct);
    }

    public async Task<(IEnumerable<Obra> Items, int Total)> GetPagedAsync(
        Guid empresaId, string? search, StatusObraEnum? status, TipoObraEnum? tipo,
        int page, int pageSize, CancellationToken ct = default)
    {
        var q = _set.Include(o => o.Endereco).Where(o => o.EmpresaId == empresaId && !o.IsDeleted);
        if (!string.IsNullOrWhiteSpace(search))
            q = q.Where(o => o.Nome.Contains(search) || o.Codigo.Contains(search) || (o.Endereco != null && o.Endereco.Cidade != null && o.Endereco.Cidade.Contains(search)));
        if (status.HasValue) q = q.Where(o => o.Status == status.Value);
        if (tipo.HasValue) q = q.Where(o => o.Tipo == tipo.Value);
        var total = await q.CountAsync(ct);
        var items = await q.OrderByDescending(o => o.CreatedAt)
            .Skip((page - 1) * pageSize).Take(pageSize).ToListAsync(ct);
        return (items, total);
    }

    public async Task<ObrasDashboardStats> GetDashboardStatsAsync(Guid empresaId, CancellationToken ct = default)
    {
        var todas = await _set.Where(o => o.EmpresaId == empresaId && !o.IsDeleted).ToListAsync(ct);
        var hoje = DateTime.Today;
        return new ObrasDashboardStats(
            todas.Count,
            todas.Count(o => o.Status == StatusObraEnum.EmAndamento),
            todas.Count(o => o.Status == StatusObraEnum.Concluida),
            todas.Count(o => o.Status == StatusObraEnum.Paralisada),
            todas.Count(o => o.Status == StatusObraEnum.EmAndamento && o.DataFimPrevista < hoje),
            todas.Sum(o => o.ValorContrato),
            todas.Sum(o => o.ValorRealizado),
            todas.Any() ? (decimal)todas.Average(o => (double)o.PercentualConcluido) : 0
        );
    }

    public async Task<IEnumerable<Obra>> GetAllByEmpresaComEnderecoAsync(Guid empresaId, CancellationToken ct = default)
        => await _set.Include(o => o.Endereco).Where(o => o.EmpresaId == empresaId).ToListAsync(ct);

    public async Task<IEnumerable<Obra>> GetUltimasObrasAsync(Guid empresaId, int count, CancellationToken ct = default)
        => await _set.Where(o => o.EmpresaId == empresaId && !o.IsDeleted)
            .OrderByDescending(o => o.CreatedAt).Take(count).ToListAsync(ct);

    public async Task AddHistoricoAsync(ObraHistorico historico, CancellationToken ct = default)
        => await _ctx.ObrasHistoricos.AddAsync(historico, ct);
}

public class FaseObraRepository : IFaseObraRepository
{
    private readonly AppDbContext _ctx;
    public FaseObraRepository(AppDbContext ctx) => _ctx = ctx;
    public async Task<FaseObra?> GetByIdAsync(Guid id, CancellationToken ct = default)
        => await _ctx.FasesObra.FirstOrDefaultAsync(f => f.Id == id, ct);
    public async Task<IEnumerable<FaseObra>> GetAllAsync(CancellationToken ct = default)
        => await _ctx.FasesObra.ToListAsync(ct);
    public async Task<IEnumerable<FaseObra>> FindAsync(Expression<Func<FaseObra, bool>> predicate, CancellationToken ct = default)
        => await _ctx.FasesObra.Where(predicate).ToListAsync(ct);
    public async Task<FaseObra?> FirstOrDefaultAsync(Expression<Func<FaseObra, bool>> predicate, CancellationToken ct = default)
        => await _ctx.FasesObra.FirstOrDefaultAsync(predicate, ct);
    public async Task<bool> ExistsAsync(Expression<Func<FaseObra, bool>> predicate, CancellationToken ct = default)
        => await _ctx.FasesObra.AnyAsync(predicate, ct);
    public async Task<int> CountAsync(Expression<Func<FaseObra, bool>>? predicate = null, CancellationToken ct = default)
        => predicate == null ? await _ctx.FasesObra.CountAsync(ct) : await _ctx.FasesObra.CountAsync(predicate, ct);
    public async Task AddAsync(FaseObra entity, CancellationToken ct = default) => await _ctx.FasesObra.AddAsync(entity, ct);
    public async Task AddRangeAsync(IEnumerable<FaseObra> entities, CancellationToken ct = default) => await _ctx.FasesObra.AddRangeAsync(entities, ct);
    public void Update(FaseObra entity) => _ctx.FasesObra.Update(entity);
    public void Remove(FaseObra entity) => _ctx.FasesObra.Remove(entity);
    public void RemoveRange(IEnumerable<FaseObra> entities) => _ctx.FasesObra.RemoveRange(entities);
    public IQueryable<FaseObra> Query() => _ctx.FasesObra.AsQueryable();
    public async Task<IEnumerable<FaseObra>> GetByObraAsync(Guid obraId, CancellationToken ct = default)
        => await _ctx.FasesObra.Where(f => f.ObraId == obraId && !f.IsDeleted).OrderBy(f => f.Ordem).ToListAsync(ct);
}
