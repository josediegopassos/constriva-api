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

// ─── Financeiro Repository ────────────────────────────────────────────────────
public class LancamentoFinanceiroRepository : TenantRepository<LancamentoFinanceiro>, ILancamentoFinanceiroRepository
{
    public LancamentoFinanceiroRepository(AppDbContext ctx) : base(ctx) { }

    public async Task<IEnumerable<LancamentoFinanceiro>> GetByObraAsync(Guid empresaId, Guid obraId, CancellationToken ct = default)
        => await _set.Where(l => l.EmpresaId == empresaId && l.ObraId == obraId).ToListAsync(ct);

    public async Task<IEnumerable<LancamentoFinanceiro>> GetVencidosAsync(Guid empresaId, CancellationToken ct = default)
        => await _set.Where(l => l.EmpresaId == empresaId && l.Status == StatusLancamentoEnum.Previsto && l.DataVencimento < DateTime.UtcNow).ToListAsync(ct);

    public async Task<IEnumerable<LancamentoFinanceiro>> GetByPeriodoAsync(Guid empresaId, DateTime inicio, DateTime fim, CancellationToken ct = default)
        => await _set.Where(l => l.EmpresaId == empresaId && l.DataVencimento >= inicio && l.DataVencimento <= fim).ToListAsync(ct);

    public async Task<decimal> GetSaldoContaAsync(Guid contaId, CancellationToken ct = default)
    {
        var receitas = await _set.Where(l => l.ContaBancariaId == contaId && l.Tipo == TipoLancamentoEnum.Receita && l.Status == StatusLancamentoEnum.Realizado).SumAsync(l => l.ValorRealizado ?? 0, ct);
        var despesas = await _set.Where(l => l.ContaBancariaId == contaId && l.Tipo == TipoLancamentoEnum.Despesa && l.Status == StatusLancamentoEnum.Realizado).SumAsync(l => l.ValorRealizado ?? 0, ct);
        return receitas - despesas;
    }

    public async Task<(IEnumerable<LancamentoFinanceiro> Items, int Total)> GetPagedAsync(
        Guid empresaId, Guid? obraId, DateTime? inicio, DateTime? fim,
        TipoLancamentoEnum? tipo, StatusLancamentoEnum? status, int page, int pageSize, CancellationToken ct = default)
    {
        var q = _set.Where(l => l.EmpresaId == empresaId && !l.IsDeleted);
        if (obraId.HasValue) q = q.Where(l => l.ObraId == obraId);
        if (inicio.HasValue) q = q.Where(l => l.DataVencimento >= inicio);
        if (fim.HasValue) q = q.Where(l => l.DataVencimento <= fim);
        if (tipo.HasValue) q = q.Where(l => l.Tipo == tipo);
        if (status.HasValue) q = q.Where(l => l.Status == status);
        var total = await q.CountAsync(ct);
        var items = await q.OrderByDescending(l => l.DataVencimento).Skip((page - 1) * pageSize).Take(pageSize).ToListAsync(ct);
        return (items, total);
    }

    public async Task<FinanceiroDashboardStats> GetDashboardStatsAsync(Guid empresaId, Guid? obraId, CancellationToken ct = default)
    {
        var q = _set.Where(l => l.EmpresaId == empresaId && !l.IsDeleted);
        if (obraId.HasValue) q = q.Where(l => l.ObraId == obraId);
        var lancamentos = await q.ToListAsync(ct);
        var hoje = DateTime.UtcNow;
        return new FinanceiroDashboardStats(
            lancamentos.Where(l => l.Tipo == TipoLancamentoEnum.Receita).Sum(l => l.Valor),
            lancamentos.Where(l => l.Tipo == TipoLancamentoEnum.Despesa).Sum(l => l.Valor),
            lancamentos.Where(l => l.Tipo == TipoLancamentoEnum.Receita && l.Status == StatusLancamentoEnum.Realizado).Sum(l => l.ValorRealizado ?? 0),
            lancamentos.Where(l => l.Tipo == TipoLancamentoEnum.Despesa && l.Status == StatusLancamentoEnum.Realizado).Sum(l => l.ValorRealizado ?? 0),
            lancamentos.Count(l => l.Status == StatusLancamentoEnum.Previsto && l.DataVencimento < hoje),
            lancamentos.Count(l => l.Status == StatusLancamentoEnum.Previsto && l.DataVencimento >= hoje && l.DataVencimento <= hoje.AddDays(7)));
    }

    public async Task<IEnumerable<FluxoMensalData>> GetFluxoMensalAsync(Guid empresaId, Guid? obraId, int meses, CancellationToken ct = default)
    {
        var inicio = DateTime.UtcNow.AddMonths(-meses);
        var q = _set.Where(l => l.EmpresaId == empresaId && !l.IsDeleted && l.DataVencimento >= inicio);
        if (obraId.HasValue) q = q.Where(l => l.ObraId == obraId);
        var lancamentos = await q.ToListAsync(ct);
        return lancamentos
            .GroupBy(l => new { l.DataVencimento.Year, l.DataVencimento.Month })
            .Select(g => new FluxoMensalData(
                g.Key.Year, g.Key.Month,
                g.Where(l => l.Tipo == TipoLancamentoEnum.Receita).Sum(l => l.Valor),
                g.Where(l => l.Tipo == TipoLancamentoEnum.Despesa).Sum(l => l.Valor)))
            .OrderBy(f => f.Ano).ThenBy(f => f.Mes);
    }
}

// ─── Financeiro – CentroCusto / ContaBancaria ─────────────────────────────────
public class CentroCustoRepository : TenantRepository<CentroCusto>, ICentroCustoRepository
{
    public CentroCustoRepository(AppDbContext ctx) : base(ctx) { }
}

public class ContaBancariaRepository : TenantRepository<ContaBancaria>, IContaBancariaRepository
{
    public ContaBancariaRepository(AppDbContext ctx) : base(ctx) { }
}
