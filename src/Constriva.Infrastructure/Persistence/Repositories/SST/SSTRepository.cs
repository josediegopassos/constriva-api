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

// ─── SST Repository ───────────────────────────────────────────────────────────
public class SSTRepository : ISSTRepository
{
    private readonly AppDbContext _ctx;
    public SSTRepository(AppDbContext ctx) => _ctx = ctx;

    public async Task<(IEnumerable<DDS> Items, int Total)> GetDDSPagedAsync(Guid empresaId, Guid? obraId, int page, int pageSize, CancellationToken ct = default)
    {
        var q = _ctx.DDSs.Where(d => d.EmpresaId == empresaId && !d.IsDeleted);
        if (obraId.HasValue) q = q.Where(d => d.ObraId == obraId);
        var total = await q.CountAsync(ct);
        var items = await q.OrderByDescending(d => d.DataRealizacao).Skip((page - 1) * pageSize).Take(pageSize).ToListAsync(ct);
        return (items, total);
    }

    public async Task AddDDSAsync(DDS dds, CancellationToken ct = default) => await _ctx.DDSs.AddAsync(dds, ct);

    public async Task<IEnumerable<RegistroAcidente>> GetAcidentesAsync(Guid empresaId, Guid? obraId, CancellationToken ct = default)
    {
        var q = _ctx.RegistrosAcidentes.Where(a => a.EmpresaId == empresaId && !a.IsDeleted);
        if (obraId.HasValue) q = q.Where(a => a.ObraId == obraId);
        return await q.OrderByDescending(a => a.DataHoraAcidente).Take(50).ToListAsync(ct);
    }

    public async Task AddAcidenteAsync(RegistroAcidente acidente, CancellationToken ct = default) => await _ctx.RegistrosAcidentes.AddAsync(acidente, ct);

    public async Task<DDS?> GetDDSByIdAsync(Guid id, Guid empresaId, CancellationToken ct = default)
        => await _ctx.DDSs.FirstOrDefaultAsync(d => d.Id == id && d.EmpresaId == empresaId && !d.IsDeleted, ct);

    public async Task<RegistroAcidente?> GetAcidenteByIdAsync(Guid id, Guid empresaId, CancellationToken ct = default)
        => await _ctx.RegistrosAcidentes.FirstOrDefaultAsync(a => a.Id == id && a.EmpresaId == empresaId && !a.IsDeleted, ct);

    public async Task<SSTIndicadoresData> GetIndicadoresAsync(Guid empresaId, Guid? obraId, CancellationToken ct = default)
    {
        var qDDS = _ctx.DDSs.Where(d => d.EmpresaId == empresaId && !d.IsDeleted);
        if (obraId.HasValue) qDDS = qDDS.Where(d => d.ObraId == obraId);
        var totalDDS = await qDDS.CountAsync(ct);

        var qAc = _ctx.RegistrosAcidentes.Where(a => a.EmpresaId == empresaId && !a.IsDeleted);
        if (obraId.HasValue) qAc = qAc.Where(a => a.ObraId == obraId);
        var acidentes = await qAc.ToListAsync(ct);

        var totalAcidentes = acidentes.Count;
        var acidentesGraves = acidentes.Count(a => a.AfastamentoMedico);
        var diasParados = acidentes.Where(a => a.DiasAfastamento.HasValue).Sum(a => a.DiasAfastamento!.Value);

        return new SSTIndicadoresData(totalDDS, totalAcidentes, acidentesGraves, diasParados, 0m, 0m);
    }

    public async Task<DateTime?> GetUltimoAcidenteAsync(Guid empresaId, Guid? obraId, CancellationToken ct = default)
    {
        var q = _ctx.RegistrosAcidentes.Where(a => a.EmpresaId == empresaId && a.Tipo != TipoAcidenteEnum.QuaseAcidente);
        if (obraId.HasValue) q = q.Where(a => a.ObraId == obraId);
        return await q.OrderByDescending(a => a.DataHoraAcidente).Select(a => (DateTime?)a.DataHoraAcidente).FirstOrDefaultAsync(ct);
    }

    public async Task<IEnumerable<EPI>> GetEPIsAsync(Guid empresaId, CancellationToken ct = default)
        => await _ctx.EPIs.Where(e => e.EmpresaId == empresaId && !e.IsDeleted).OrderBy(e => e.Nome).ToListAsync(ct);

    public async Task<EPI?> GetEPIByIdAsync(Guid id, Guid empresaId, CancellationToken ct = default)
        => await _ctx.EPIs.FirstOrDefaultAsync(e => e.Id == id && e.EmpresaId == empresaId && !e.IsDeleted, ct);

    public async Task AddEPIAsync(EPI epi, CancellationToken ct = default)
        => await _ctx.EPIs.AddAsync(epi, ct);
}
