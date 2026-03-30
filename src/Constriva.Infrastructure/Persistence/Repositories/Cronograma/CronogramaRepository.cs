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

// ─── Cronograma Repository ────────────────────────────────────────────────────
public class CronogramaRepository : ICronogramaRepository
{
    private readonly AppDbContext _ctx;
    public CronogramaRepository(AppDbContext ctx) => _ctx = ctx;

    public async Task<CronogramaObra?> GetByObraAsync(Guid obraId, Guid empresaId, CancellationToken ct = default)
        => await _ctx.Cronogramas.FirstOrDefaultAsync(c => c.ObraId == obraId && c.EmpresaId == empresaId && c.Ativo && !c.IsDeleted, ct);

    public async Task<CronogramaObra?> GetByIdAndEmpresaAsync(Guid id, Guid empresaId, CancellationToken ct = default)
        => await _ctx.Cronogramas.FirstOrDefaultAsync(c => c.Id == id && c.EmpresaId == empresaId && !c.IsDeleted, ct);

    public async Task<CronogramaObra?> GetWithAtividadesAsync(Guid obraId, Guid empresaId, CancellationToken ct = default)
        => await _ctx.Cronogramas
            .Include(c => c.Obra)
            .Include(c => c.Atividades.Where(a => !a.IsDeleted))
                .ThenInclude(a => a.Predecessoras)
            .AsSplitQuery()
            .FirstOrDefaultAsync(c => c.ObraId == obraId && c.EmpresaId == empresaId && c.Ativo && !c.IsDeleted, ct);

    public async Task<IEnumerable<CronogramaObra>> GetAllWithAtividadesAsync(Guid empresaId, CancellationToken ct = default)
        => await _ctx.Cronogramas
            .Include(c => c.Obra)
            .Include(c => c.Atividades.Where(a => !a.IsDeleted))
                .ThenInclude(a => a.Predecessoras)
            .AsSplitQuery()
            .Where(c => c.EmpresaId == empresaId && c.Ativo && !c.IsDeleted)
            .ToListAsync(ct);

    public async Task<CronogramaObra?> GetByIdDetalhadoAsync(Guid id, Guid empresaId, CancellationToken ct = default)
        => await _ctx.Cronogramas
            .Include(c => c.Obra)
            .Include(c => c.Atividades.Where(a => !a.IsDeleted))
                .ThenInclude(a => a.Predecessoras)
            .Include(c => c.Atividades.Where(a => !a.IsDeleted))
                .ThenInclude(a => a.Sucessoras)
            .Include(c => c.Atividades.Where(a => !a.IsDeleted))
                .ThenInclude(a => a.Recursos)
            .AsSplitQuery()
            .FirstOrDefaultAsync(c => c.Id == id && c.EmpresaId == empresaId && !c.IsDeleted, ct);

    public async Task AddCronogramaAsync(CronogramaObra cronograma, CancellationToken ct = default)
        => await _ctx.Cronogramas.AddAsync(cronograma, ct);

    public async Task<AtividadeCronograma?> GetAtividadeByIdAsync(Guid id, Guid empresaId, CancellationToken ct = default)
        => await _ctx.AtividadesCronograma.FirstOrDefaultAsync(a => a.Id == id && a.EmpresaId == empresaId, ct);

    public async Task AddAtividadeAsync(AtividadeCronograma atividade, CancellationToken ct = default)
        => await _ctx.AtividadesCronograma.AddAsync(atividade, ct);

    public async Task<int> GetMaxOrdemAsync(Guid cronogramaId, CancellationToken ct = default)
        => await _ctx.AtividadesCronograma.Where(a => a.CronogramaId == cronogramaId).MaxAsync(a => (int?)a.Ordem, ct) ?? 0;

    public async Task<IEnumerable<CurvaSPonto>> GetCurvaSAsync(Guid cronogramaId, Guid empresaId, CancellationToken ct = default)
        => await _ctx.CurvaSPontos.Where(p => p.CronogramaId == cronogramaId && p.EmpresaId == empresaId).OrderBy(p => p.DataReferencia).ToListAsync(ct);

    public async Task RemoveCurvaSAsync(Guid cronogramaId, Guid empresaId, CancellationToken ct = default)
    {
        var pontos = await _ctx.CurvaSPontos.Where(p => p.CronogramaId == cronogramaId && p.EmpresaId == empresaId).ToListAsync(ct);
        _ctx.CurvaSPontos.RemoveRange(pontos);
    }

    public async Task AddCurvaSRangeAsync(IEnumerable<CurvaSPonto> pontos, CancellationToken ct = default)
        => await _ctx.CurvaSPontos.AddRangeAsync(pontos, ct);

    public async Task<IEnumerable<VinculoAtividade>> GetVinculosAsync(Guid cronogramaId, CancellationToken ct = default)
        => await _ctx.VinculosAtividades
            .Where(v => _ctx.AtividadesCronograma
                .Where(a => a.CronogramaId == cronogramaId)
                .Select(a => a.Id)
                .Contains(v.AtividadeDestinoId))
            .ToListAsync(ct);
}
