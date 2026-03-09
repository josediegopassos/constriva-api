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

// ─── Qualidade Repository ─────────────────────────────────────────────────────
public class QualidadeRepository : IQualidadeRepository
{
    private readonly AppDbContext _ctx;
    public QualidadeRepository(AppDbContext ctx) => _ctx = ctx;

    public async Task<(IEnumerable<Inspecao> Items, int Total)> GetInspecoesPagedAsync(
        Guid empresaId, Guid? obraId, StatusInspecaoEnum? status, int page, int pageSize, CancellationToken ct = default)
    {
        var q = _ctx.Inspecoes.Where(i => i.EmpresaId == empresaId && !i.IsDeleted);
        if (obraId.HasValue) q = q.Where(i => i.ObraId == obraId);
        if (status.HasValue) q = q.Where(i => i.Status == status);
        var total = await q.CountAsync(ct);
        var items = await q.OrderByDescending(i => i.DataProgramada).Skip((page - 1) * pageSize).Take(pageSize).ToListAsync(ct);
        return (items, total);
    }

    public async Task AddInspecaoAsync(Inspecao inspecao, CancellationToken ct = default) => await _ctx.Inspecoes.AddAsync(inspecao, ct);

    public async Task<Inspecao?> GetInspecaoByIdAsync(Guid id, Guid empresaId, CancellationToken ct = default)
        => await _ctx.Inspecoes.FirstOrDefaultAsync(i => i.Id == id && i.EmpresaId == empresaId, ct);

    public async Task<(IEnumerable<NaoConformidade> Items, int Total)> GetNCsPagedAsync(
        Guid empresaId, Guid? obraId, StatusNaoConformidadeEnum? status, int page, int pageSize, CancellationToken ct = default)
    {
        var q = _ctx.NaoConformidades.Where(n => n.EmpresaId == empresaId && !n.IsDeleted);
        if (obraId.HasValue) q = q.Where(n => n.ObraId == obraId);
        if (status.HasValue) q = q.Where(n => n.Status == status);
        var total = await q.CountAsync(ct);
        var items = await q.OrderByDescending(n => n.DataAbertura).Skip((page - 1) * pageSize).Take(pageSize).ToListAsync(ct);
        return (items, total);
    }

    public async Task AddNCAsync(NaoConformidade nc, CancellationToken ct = default) => await _ctx.NaoConformidades.AddAsync(nc, ct);

    public async Task<NaoConformidade?> GetNCByIdAsync(Guid id, Guid empresaId, CancellationToken ct = default)
        => await _ctx.NaoConformidades.FirstOrDefaultAsync(n => n.Id == id && n.EmpresaId == empresaId, ct);

    public async Task<IEnumerable<FVS>> GetFVSsAsync(Guid empresaId, Guid? obraId, CancellationToken ct = default)
    {
        var q = _ctx.FVSs.Where(f => f.EmpresaId == empresaId && !f.IsDeleted);
        if (obraId.HasValue) q = q.Where(f => f.ObraId == obraId);
        return await q.OrderByDescending(f => f.DataVerificacao).Take(50).ToListAsync(ct);
    }

    public async Task<FVS?> GetFVSByIdAsync(Guid id, Guid empresaId, CancellationToken ct = default)
        => await _ctx.FVSs.FirstOrDefaultAsync(f => f.Id == id && f.EmpresaId == empresaId && !f.IsDeleted, ct);

    public async Task<int> CountInspecoesAsync(Guid empresaId, CancellationToken ct = default)
        => await _ctx.Inspecoes.CountAsync(i => i.EmpresaId == empresaId && !i.IsDeleted, ct);

    public async Task<int> CountNCsAsync(Guid empresaId, CancellationToken ct = default)
        => await _ctx.NaoConformidades.CountAsync(n => n.EmpresaId == empresaId && !n.IsDeleted, ct);
}
