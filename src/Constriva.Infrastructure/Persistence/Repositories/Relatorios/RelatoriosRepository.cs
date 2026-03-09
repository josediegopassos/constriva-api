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

// ─── Relatórios Repository ────────────────────────────────────────────────────
public class RelatoriosRepository : IRelatoriosRepository
{
    private readonly AppDbContext _ctx;
    public RelatoriosRepository(AppDbContext ctx) => _ctx = ctx;

    public async Task<IEnumerable<Obra>> GetObrasParaRelatorioAsync(Guid empresaId, Guid? obraId, CancellationToken ct = default)
    {
        var q = _ctx.Obras.Where(o => o.EmpresaId == empresaId && !o.IsDeleted);
        if (obraId.HasValue) q = q.Where(o => o.Id == obraId);
        return await q.ToListAsync(ct);
    }

    public async Task<IEnumerable<LancamentoFinanceiro>> GetLancamentosParaRelatorioAsync(Guid empresaId, Guid? obraId, CancellationToken ct = default)
    {
        var q = _ctx.LancamentosFinanceiros.Where(l => l.EmpresaId == empresaId && !l.IsDeleted);
        if (obraId.HasValue) q = q.Where(l => l.ObraId == obraId);
        return await q.ToListAsync(ct);
    }

    public async Task<IEnumerable<AtividadeCronograma>> GetAtividadesParaKPIAsync(Guid empresaId, Guid obraId, CancellationToken ct = default)
        => await _ctx.Cronogramas
            .Include(c => c.Atividades.Where(a => !a.IsDeleted))
            .Where(c => c.ObraId == obraId && c.EmpresaId == empresaId && c.Ativo && !c.IsDeleted)
            .SelectMany(c => c.Atividades.Where(a => !a.IsDeleted))
            .ToListAsync(ct);
}
