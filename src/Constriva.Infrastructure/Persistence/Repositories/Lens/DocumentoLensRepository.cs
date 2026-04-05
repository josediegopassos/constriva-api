using Microsoft.EntityFrameworkCore;
using Constriva.Domain.Entities.Lens;
using Constriva.Domain.Enums;
using Constriva.Domain.Interfaces.Repositories;

namespace Constriva.Infrastructure.Persistence.Repositories;

public class DocumentoLensRepository : IDocumentoLensRepository
{
    private readonly AppDbContext _ctx;
    public DocumentoLensRepository(AppDbContext ctx) => _ctx = ctx;

    public async Task<(List<DocumentoLens> Items, int TotalCount)> GetProcessamentosPagedAsync(
        Guid empresaId, Guid? obraId, StatusProcessamentoLensEnum? status,
        TipoDocumentoLensEnum? tipoDocumento, DateTime? de, DateTime? ate,
        int page, int pageSize, CancellationToken ct)
    {
        var query = _ctx.DocumentosLens
            .Include(d => d.Itens)
            .Where(d => d.EmpresaId == empresaId && !d.IsDeleted)
            .AsQueryable();

        if (obraId.HasValue)
            query = query.Where(d => d.ObraId == obraId.Value);

        if (status.HasValue)
            query = query.Where(d => d.Status == status.Value);

        if (tipoDocumento.HasValue)
            query = query.Where(d => d.TipoDocumentoDeclarado == tipoDocumento.Value);

        if (de.HasValue)
            query = query.Where(d => d.CreatedAt >= de.Value);

        if (ate.HasValue)
            query = query.Where(d => d.CreatedAt <= ate.Value);

        var total = await query.CountAsync(ct);

        var items = await query
            .OrderByDescending(d => d.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);

        return (items, total);
    }

    public async Task<DocumentoLens?> GetByIdWithItensAsync(Guid id, Guid empresaId, CancellationToken ct)
    {
        return await _ctx.DocumentosLens
            .Include(d => d.Itens)
            .FirstOrDefaultAsync(d => d.Id == id && d.EmpresaId == empresaId, ct);
    }

    public async Task<DocumentoLens?> GetResultadoWithMatchingAsync(Guid id, Guid empresaId, CancellationToken ct)
    {
        return await _ctx.DocumentosLens
            .Include(d => d.Itens)
            .FirstOrDefaultAsync(d => d.Id == id && d.EmpresaId == empresaId, ct);
    }

    public async Task<(int Total, int Sucesso, int Erro, float ConfidenceMedio, int TempoMedio, int TotalItens, int TotalConsolidados)> GetResumoAsync(
        Guid empresaId, DateTime de, DateTime ate, CancellationToken ct)
    {
        var docs = await _ctx.DocumentosLens
            .Include(d => d.Itens)
            .Where(d => d.EmpresaId == empresaId && !d.IsDeleted)
            .Where(d => d.CreatedAt >= de && d.CreatedAt <= ate)
            .ToListAsync(ct);

        var total = docs.Count;
        var sucesso = docs.Count(d =>
            d.Status == StatusProcessamentoLensEnum.Aprovado ||
            d.Status == StatusProcessamentoLensEnum.Consolidado ||
            d.Status == StatusProcessamentoLensEnum.AguardandoRevisao ||
            d.Status == StatusProcessamentoLensEnum.EmRevisao);
        var erro = docs.Count(d => d.Status == StatusProcessamentoLensEnum.Erro);

        var confidenceMedio = docs.Where(d => d.ConfidenceScore.HasValue)
            .Select(d => d.ConfidenceScore!.Value)
            .DefaultIfEmpty(0f)
            .Average();

        var tempoMedio = (int)docs.Where(d => d.TempoProcessamentoMs.HasValue)
            .Select(d => d.TempoProcessamentoMs!.Value)
            .DefaultIfEmpty(0)
            .Average();

        var totalItens = docs.Sum(d => d.Itens.Count);
        var totalConsolidados = docs
            .Where(d => d.Status == StatusProcessamentoLensEnum.Consolidado)
            .Sum(d => d.Itens.Count(i => i.Status == StatusItemLensEnum.Aprovado));

        return (total, sucesso, erro, confidenceMedio, tempoMedio, totalItens, totalConsolidados);
    }

    public async Task<List<(TipoDocumentoLensEnum TipoDocumento, int Total, int Sucesso, int Erro, float ConfidenceMedio)>> GetPorTipoAsync(
        Guid empresaId, DateTime? de, DateTime? ate, CancellationToken ct)
    {
        var query = _ctx.DocumentosLens
            .Where(d => d.EmpresaId == empresaId && !d.IsDeleted)
            .AsQueryable();

        if (de.HasValue)
            query = query.Where(d => d.CreatedAt >= de.Value);

        if (ate.HasValue)
            query = query.Where(d => d.CreatedAt <= ate.Value);

        var grupos = await query
            .GroupBy(d => d.TipoDocumentoDeclarado)
            .Select(g => new
            {
                Tipo = g.Key,
                Total = g.Count(),
                Sucesso = g.Count(d =>
                    d.Status == StatusProcessamentoLensEnum.Aprovado ||
                    d.Status == StatusProcessamentoLensEnum.Consolidado ||
                    d.Status == StatusProcessamentoLensEnum.AguardandoRevisao ||
                    d.Status == StatusProcessamentoLensEnum.EmRevisao),
                Erro = g.Count(d => d.Status == StatusProcessamentoLensEnum.Erro),
                ConfidenceMedio = g.Where(d => d.ConfidenceScore.HasValue)
                    .Average(d => (float?)d.ConfidenceScore) ?? 0f
            })
            .ToListAsync(ct);

        return grupos.Select(g => (g.Tipo, g.Total, g.Sucesso, g.Erro, g.ConfidenceMedio)).ToList();
    }

    public async Task<List<(DateTime Data, float ConfidenceMedio, int TotalDocumentos)>> GetTendenciaConfidenceAsync(
        Guid empresaId, DateTime? de, DateTime? ate, CancellationToken ct)
    {
        var query = _ctx.DocumentosLens
            .Where(d => d.EmpresaId == empresaId && !d.IsDeleted && d.ConfidenceScore.HasValue);

        if (de.HasValue)
            query = query.Where(d => d.CreatedAt >= de.Value);

        if (ate.HasValue)
            query = query.Where(d => d.CreatedAt <= ate.Value);

        var tendencia = await query
            .GroupBy(d => d.CreatedAt.Date)
            .Select(g => new
            {
                Data = g.Key,
                ConfidenceMedio = g.Average(d => d.ConfidenceScore!.Value),
                TotalDocumentos = g.Count()
            })
            .OrderBy(t => t.Data)
            .ToListAsync(ct);

        return tendencia.Select(t => (t.Data, t.ConfidenceMedio, t.TotalDocumentos)).ToList();
    }

    public async Task<List<(string Warning, int Frequencia)>> GetWarningsFrequentesAsync(
        Guid empresaId, int limite, CancellationToken ct)
    {
        var docs = await _ctx.DocumentosLens
            .Where(d => d.EmpresaId == empresaId && !d.IsDeleted && d.Warnings.Count > 0)
            .Select(d => d.Warnings)
            .ToListAsync(ct);

        return docs
            .SelectMany(w => w)
            .GroupBy(w => w)
            .Select(g => (g.Key, g.Count()))
            .OrderByDescending(w => w.Item2)
            .Take(limite)
            .ToList();
    }
}
