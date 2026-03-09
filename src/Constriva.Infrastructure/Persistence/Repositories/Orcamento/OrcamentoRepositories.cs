using Microsoft.EntityFrameworkCore;
using Constriva.Domain.Entities.Orcamento;
using Constriva.Domain.Enums;
using Constriva.Domain.Interfaces.Repositories;
using Constriva.Infrastructure.Persistence;

namespace Constriva.Infrastructure.Persistence.Repositories.Orcamento;

// ─── Orçamento Repository ─────────────────────────────────────────────────────

public class OrcamentoRepository : IOrcamentoRepository
{
    private readonly AppDbContext _ctx;

    public OrcamentoRepository(AppDbContext ctx) => _ctx = ctx;

    public async Task<IEnumerable<Domain.Entities.Orcamento.Orcamento>> GetByEmpresaAsync(
        Guid empresaId, Guid? obraId, int page, int pageSize, CancellationToken ct = default)
    {
        var query = _ctx.Orcamentos
            .Include(o => o.Grupos.Where(g => !g.IsDeleted))
            .Where(o => o.EmpresaId == empresaId && !o.IsDeleted);
        if (obraId.HasValue)
            query = query.Where(o => o.ObraId == obraId.Value);
        return await query
            .OrderByDescending(o => o.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);
    }

    public async Task<int> CountByEmpresaAsync(Guid empresaId, Guid? obraId, CancellationToken ct = default)
    {
        var query = _ctx.Orcamentos.Where(o => o.EmpresaId == empresaId && !o.IsDeleted);
        if (obraId.HasValue)
            query = query.Where(o => o.ObraId == obraId.Value);
        return await query.CountAsync(ct);
    }

    public async Task<IEnumerable<Domain.Entities.Orcamento.Orcamento>> GetByObraAsync(
        Guid obraId, Guid empresaId, CancellationToken ct = default)
        => await _ctx.Orcamentos
            .Include(o => o.Grupos.Where(g => !g.IsDeleted))
                .ThenInclude(g => g.Itens.Where(i => !i.IsDeleted))
            .Where(o => o.ObraId == obraId && o.EmpresaId == empresaId && !o.IsDeleted)
            .OrderByDescending(o => o.Versao)
            .ToListAsync(ct);

    public async Task<Domain.Entities.Orcamento.Orcamento?> GetByIdAsync(
        Guid id, Guid empresaId, CancellationToken ct = default)
        => await _ctx.Orcamentos
            .FirstOrDefaultAsync(o => o.Id == id && o.EmpresaId == empresaId && !o.IsDeleted, ct);

    public async Task<Domain.Entities.Orcamento.Orcamento?> GetWithGruposItensAsync(
        Guid id, Guid empresaId, CancellationToken ct = default)
        => await _ctx.Orcamentos
            .Include(o => o.Grupos.Where(g => !g.IsDeleted))
                .ThenInclude(g => g.SubGrupos.Where(sg => !sg.IsDeleted))
                    .ThenInclude(sg => sg.Itens.Where(i => !i.IsDeleted))
            .Include(o => o.Grupos.Where(g => !g.IsDeleted))
                .ThenInclude(g => g.Itens.Where(i => !i.IsDeleted))
            .Include(o => o.Historicos.OrderByDescending(h => h.CreatedAt))
            .FirstOrDefaultAsync(o => o.Id == id && o.EmpresaId == empresaId && !o.IsDeleted, ct);

    public async Task AddAsync(
        Domain.Entities.Orcamento.Orcamento orcamento, CancellationToken ct = default)
        => await _ctx.Orcamentos.AddAsync(orcamento, ct);

    public void Update(Domain.Entities.Orcamento.Orcamento orcamento)
        => _ctx.Orcamentos.Update(orcamento);

    public void Remove(Domain.Entities.Orcamento.Orcamento orcamento)
        => _ctx.Orcamentos.Remove(orcamento);

    public async Task<int> GetMaxVersaoAsync(
        Guid obraId, Guid empresaId, CancellationToken ct = default)
        => await _ctx.Orcamentos
            .Where(o => o.ObraId == obraId && o.EmpresaId == empresaId)
            .MaxAsync(o => (int?)o.Versao, ct) ?? 0;

    public async Task<bool> ExistsLinhaDBaseAsync(
        Guid obraId, Guid empresaId, CancellationToken ct = default)
        => await _ctx.Orcamentos
            .AnyAsync(o => o.ObraId == obraId && o.EmpresaId == empresaId
                           && o.ELinhaDBase && !o.IsDeleted, ct);

    public async Task AddHistoricoAsync(
        OrcamentoHistorico historico, CancellationToken ct = default)
        => await _ctx.OrcamentosHistoricos.AddAsync(historico, ct);

    public async Task<IEnumerable<OrcamentoHistorico>> GetHistoricoAsync(
        Guid orcamentoId, Guid empresaId, CancellationToken ct = default)
        => await _ctx.OrcamentosHistoricos
            .Where(h => h.OrcamentoId == orcamentoId && h.EmpresaId == empresaId)
            .OrderByDescending(h => h.CreatedAt)
            .ToListAsync(ct);

    public async Task<int> CountByEmpresaAsync(
        Guid empresaId, StatusOrcamentoEnum? status = null, CancellationToken ct = default)
    {
        var query = _ctx.Orcamentos.Where(o => o.EmpresaId == empresaId && !o.IsDeleted);
        if (status.HasValue) query = query.Where(o => o.Status == status);
        return await query.CountAsync(ct);
    }

    public async Task<decimal> GetValorTotalAprovadosAsync(
        Guid obraId, Guid empresaId, CancellationToken ct = default)
        => await _ctx.Orcamentos
            .Where(o => o.ObraId == obraId && o.EmpresaId == empresaId
                        && o.Status == StatusOrcamentoEnum.Aprovado && !o.IsDeleted)
            .SumAsync(o => o.ValorTotal, ct);

    public async Task<OrcamentoDashboardStats> GetDashboardStatsAsync(
        Guid empresaId, CancellationToken ct = default)
    {
        var orcamentos = await _ctx.Orcamentos
            .Where(o => o.EmpresaId == empresaId && !o.IsDeleted)
            .ToListAsync(ct);
        return new OrcamentoDashboardStats(
            orcamentos.Count,
            orcamentos.Count(o => o.Status == StatusOrcamentoEnum.Rascunho),
            orcamentos.Count(o => o.Status == StatusOrcamentoEnum.EmAnalise),
            orcamentos.Count(o => o.Status == StatusOrcamentoEnum.Aprovado),
            orcamentos.Where(o => o.Status == StatusOrcamentoEnum.Aprovado).Sum(o => o.ValorTotal));
    }
}

// ─── Grupo Orçamento Repository ───────────────────────────────────────────────

public class GrupoOrcamentoRepository : IGrupoOrcamentoRepository
{
    private readonly AppDbContext _ctx;

    public GrupoOrcamentoRepository(AppDbContext ctx) => _ctx = ctx;

    public async Task<GrupoOrcamento?> GetByIdAsync(
        Guid id, Guid empresaId, CancellationToken ct = default)
        => await _ctx.GruposOrcamento
            .FirstOrDefaultAsync(g => g.Id == id && g.EmpresaId == empresaId && !g.IsDeleted, ct);

    public async Task<GrupoOrcamento?> GetWithItensAsync(
        Guid id, Guid empresaId, CancellationToken ct = default)
        => await _ctx.GruposOrcamento
            .Include(g => g.Itens.Where(i => !i.IsDeleted))
            .Include(g => g.SubGrupos.Where(sg => !sg.IsDeleted))
            .FirstOrDefaultAsync(g => g.Id == id && g.EmpresaId == empresaId && !g.IsDeleted, ct);

    public async Task<IEnumerable<GrupoOrcamento>> GetByOrcamentoAsync(
        Guid orcamentoId, CancellationToken ct = default)
        => await _ctx.GruposOrcamento
            .Include(g => g.Itens.Where(i => !i.IsDeleted))
            .Include(g => g.SubGrupos.Where(sg => !sg.IsDeleted))
                .ThenInclude(sg => sg.Itens.Where(i => !i.IsDeleted))
            .Where(g => g.OrcamentoId == orcamentoId && !g.IsDeleted)
            .OrderBy(g => g.Ordem)
            .ToListAsync(ct);

    public async Task AddAsync(GrupoOrcamento grupo, CancellationToken ct = default)
        => await _ctx.GruposOrcamento.AddAsync(grupo, ct);

    public void Update(GrupoOrcamento grupo) => _ctx.GruposOrcamento.Update(grupo);

    public void Remove(GrupoOrcamento grupo) => _ctx.GruposOrcamento.Remove(grupo);

    public async Task<int> GetMaxOrdemAsync(Guid orcamentoId, CancellationToken ct = default)
        => await _ctx.GruposOrcamento
            .Where(g => g.OrcamentoId == orcamentoId && g.GrupoPaiId == null)
            .MaxAsync(g => (int?)g.Ordem, ct) ?? 0;

    public async Task RecalcularTotaisAsync(
        Guid orcamentoId, Guid empresaId, CancellationToken ct = default)
    {
        var grupos = await _ctx.GruposOrcamento
            .Include(g => g.Itens.Where(i => !i.IsDeleted))
            .Where(g => g.OrcamentoId == orcamentoId && !g.IsDeleted)
            .ToListAsync(ct);

        decimal totalOrcamento = 0;

        foreach (var grupo in grupos)
        {
            grupo.ValorTotal = grupo.Itens?.Sum(i => i.Quantidade * i.CustoUnitario) ?? 0;
            totalOrcamento += grupo.ValorTotal;
            _ctx.GruposOrcamento.Update(grupo);
        }

        // Calcular percentuais
        foreach (var grupo in grupos)
        {
            grupo.PercentualTotal = totalOrcamento > 0
                ? grupo.ValorTotal / totalOrcamento * 100
                : 0;
            _ctx.GruposOrcamento.Update(grupo);
        }

        var orcamento = await _ctx.Orcamentos
            .FirstOrDefaultAsync(o => o.Id == orcamentoId && o.EmpresaId == empresaId, ct);

        if (orcamento != null)
        {
            orcamento.ValorCustoDirecto = totalOrcamento;
            orcamento.ValorBDI = totalOrcamento * orcamento.BDI / 100;
            orcamento.ValorTotal = orcamento.ValorCustoDirecto + orcamento.ValorBDI;
            orcamento.UpdatedAt = DateTime.UtcNow;
            _ctx.Orcamentos.Update(orcamento);
        }
    }
}

// ─── Item Orçamento Repository ────────────────────────────────────────────────

public class ItemOrcamentoRepository : IItemOrcamentoRepository
{
    private readonly AppDbContext _ctx;

    public ItemOrcamentoRepository(AppDbContext ctx) => _ctx = ctx;

    public async Task<ItemOrcamento?> GetByIdAsync(
        Guid id, Guid empresaId, CancellationToken ct = default)
        => await _ctx.ItensOrcamento
            .FirstOrDefaultAsync(i => i.Id == id && i.EmpresaId == empresaId && !i.IsDeleted, ct);

    public async Task<IEnumerable<ItemOrcamento>> GetByGrupoAsync(
        Guid grupoId, CancellationToken ct = default)
        => await _ctx.ItensOrcamento
            .Where(i => i.GrupoId == grupoId && !i.IsDeleted)
            .OrderBy(i => i.Ordem)
            .ToListAsync(ct);

    public async Task<IEnumerable<ItemOrcamento>> GetByOrcamentoAsync(
        Guid orcamentoId, CancellationToken ct = default)
        => await _ctx.ItensOrcamento
            .Where(i => i.OrcamentoId == orcamentoId && !i.IsDeleted)
            .OrderBy(i => i.Ordem)
            .ToListAsync(ct);

    public async Task AddAsync(ItemOrcamento item, CancellationToken ct = default)
        => await _ctx.ItensOrcamento.AddAsync(item, ct);

    public void Update(ItemOrcamento item) => _ctx.ItensOrcamento.Update(item);

    public void Remove(ItemOrcamento item) => _ctx.ItensOrcamento.Remove(item);

    public async Task<int> GetMaxOrdemAsync(Guid grupoId, CancellationToken ct = default)
        => await _ctx.ItensOrcamento
            .Where(i => i.GrupoId == grupoId)
            .MaxAsync(i => (int?)i.Ordem, ct) ?? 0;

    public async Task<int> CountByOrcamentoAsync(
        Guid orcamentoId, CancellationToken ct = default)
        => await _ctx.ItensOrcamento
            .CountAsync(i => i.OrcamentoId == orcamentoId && !i.IsDeleted, ct);
}

// ─── Composição Orçamento Repository ─────────────────────────────────────────

public class ComposicaoOrcamentoRepository : IComposicaoOrcamentoRepository
{
    private readonly AppDbContext _ctx;

    public ComposicaoOrcamentoRepository(AppDbContext ctx) => _ctx = ctx;

    public async Task<IEnumerable<ComposicaoOrcamento>> GetByOrcamentoAsync(
        Guid orcamentoId, Guid empresaId, CancellationToken ct = default)
        => await _ctx.ComposicoesOrcamento
            .Include(c => c.Insumos.Where(i => !i.IsDeleted))
            .Where(c => c.OrcamentoId == orcamentoId && c.EmpresaId == empresaId && !c.IsDeleted)
            .OrderBy(c => c.Codigo)
            .ToListAsync(ct);

    public async Task<ComposicaoOrcamento?> GetByIdAsync(
        Guid id, Guid empresaId, CancellationToken ct = default)
        => await _ctx.ComposicoesOrcamento
            .Include(c => c.Insumos.Where(i => !i.IsDeleted))
            .FirstOrDefaultAsync(c => c.Id == id && c.EmpresaId == empresaId && !c.IsDeleted, ct);

    public async Task<ComposicaoOrcamento?> GetWithInsumosAsync(
        Guid id, Guid empresaId, CancellationToken ct = default)
        => await _ctx.ComposicoesOrcamento
            .Include(c => c.Insumos.Where(i => !i.IsDeleted))
            .FirstOrDefaultAsync(c => c.Id == id && c.EmpresaId == empresaId && !c.IsDeleted, ct);

    public async Task<bool> CodigoExisteAsync(
        Guid orcamentoId, string codigo, Guid? excludeId, CancellationToken ct = default)
    {
        var query = _ctx.ComposicoesOrcamento
            .Where(c => c.OrcamentoId == orcamentoId && c.Codigo == codigo && !c.IsDeleted);
        if (excludeId.HasValue) query = query.Where(c => c.Id != excludeId.Value);
        return await query.AnyAsync(ct);
    }

    public async Task AddAsync(ComposicaoOrcamento composicao, CancellationToken ct = default)
        => await _ctx.ComposicoesOrcamento.AddAsync(composicao, ct);

    public void Update(ComposicaoOrcamento composicao) => _ctx.ComposicoesOrcamento.Update(composicao);

    public void Remove(ComposicaoOrcamento composicao) => _ctx.ComposicoesOrcamento.Remove(composicao);

    public async Task AddInsumoAsync(InsumoComposicao insumo, CancellationToken ct = default)
        => await _ctx.InsumosComposicao.AddAsync(insumo, ct);

    public async Task<IEnumerable<InsumoComposicao>> GetInsumosByComposicaoAsync(
        Guid composicaoId, CancellationToken ct = default)
        => await _ctx.InsumosComposicao
            .Where(i => i.ComposicaoId == composicaoId && !i.IsDeleted)
            .ToListAsync(ct);
}
