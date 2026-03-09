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

// ─── GED Repository ───────────────────────────────────────────────────────────
public class GEDRepository : IGEDRepository
{
    private readonly AppDbContext _ctx;
    public GEDRepository(AppDbContext ctx) => _ctx = ctx;

    public async Task<IEnumerable<PastaDocumento>> GetPastasAsync(Guid empresaId, Guid? obraId, CancellationToken ct = default)
    {
        var q = _ctx.PastasDocumentos
            .Include(p => p.SubPastas.Where(s => s.Ativo && !s.IsDeleted))
            .Where(p => p.EmpresaId == empresaId && p.Ativo && !p.IsDeleted && p.PastaPaiId == null);
        if (obraId.HasValue) q = q.Where(p => p.ObraId == obraId || p.ObraId == null);
        return await q.OrderBy(p => p.Nome).ToListAsync(ct);
    }

    public async Task<PastaDocumento?> GetPastaAsync(Guid id, Guid empresaId, CancellationToken ct = default)
        => await _ctx.PastasDocumentos.FirstOrDefaultAsync(p => p.Id == id && p.EmpresaId == empresaId && !p.IsDeleted, ct);

    public async Task<(IEnumerable<DocumentoGED> Items, int Total)> GetDocumentosPagedAsync(
        Guid empresaId, Guid? pastaId, string? search, TipoDocumentoGEDEnum? tipo, int page, int pageSize, CancellationToken ct = default)
    {
        var q = _ctx.DocumentosGED.Include(d => d.Pasta).Where(d => d.EmpresaId == empresaId && !d.IsDeleted);
        if (pastaId.HasValue) q = q.Where(d => d.PastaId == pastaId);
        if (!string.IsNullOrWhiteSpace(search)) q = q.Where(d => d.Titulo.Contains(search) || d.Codigo.Contains(search));
        if (tipo.HasValue) q = q.Where(d => d.Tipo == tipo);
        var total = await q.CountAsync(ct);
        var items = await q.OrderByDescending(d => d.CreatedAt).Skip((page - 1) * pageSize).Take(pageSize).ToListAsync(ct);
        return (items, total);
    }

    public async Task<DocumentoGED?> GetDocumentoByIdAsync(Guid id, Guid empresaId, CancellationToken ct = default)
        => await _ctx.DocumentosGED.Include(d => d.Pasta).FirstOrDefaultAsync(d => d.Id == id && d.EmpresaId == empresaId, ct);

    public async Task AddDocumentoAsync(DocumentoGED doc, CancellationToken ct = default) => await _ctx.DocumentosGED.AddAsync(doc, ct);
    public async Task AddPastaAsync(PastaDocumento pasta, CancellationToken ct = default) => await _ctx.PastasDocumentos.AddAsync(pasta, ct);
}
