using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Constriva.Domain.Entities.Common;
using Constriva.Domain.Entities.Tenant;
using Constriva.Domain.Entities.Obras;
using Constriva.Domain.Entities.Estoque;
using Constriva.Domain.Entities.Compras;
using Constriva.Domain.Entities.Fornecedores;
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

// ─── Compras Repository ───────────────────────────────────────────────────────
public class ComprasRepository : IComprasRepository
{
    private readonly AppDbContext _ctx;
    public ComprasRepository(AppDbContext ctx) => _ctx = ctx;

    public async Task<(IEnumerable<PedidoCompra> Items, int Total)> GetPedidosPagedAsync(
        Guid empresaId, Guid? obraId, StatusPedidoCompraEnum? status, int page, int pageSize, CancellationToken ct = default)
    {
        var q = _ctx.PedidosCompra.Include(p => p.Fornecedor).Include(p => p.Obra).Where(p => p.EmpresaId == empresaId && !p.IsDeleted);
        if (obraId.HasValue) q = q.Where(p => p.ObraId == obraId);
        if (status.HasValue) q = q.Where(p => p.Status == status);
        var total = await q.CountAsync(ct);
        var items = await q.OrderByDescending(p => p.CreatedAt).Skip((page - 1) * pageSize).Take(pageSize).ToListAsync(ct);
        return (items, total);
    }

    public async Task<PedidoCompra?> GetPedidoByIdAsync(Guid id, Guid empresaId, CancellationToken ct = default)
        => await _ctx.PedidosCompra.Include(p => p.Itens).FirstOrDefaultAsync(p => p.Id == id && p.EmpresaId == empresaId, ct);

    public async Task AddPedidoAsync(PedidoCompra pedido, CancellationToken ct = default)
        => await _ctx.PedidosCompra.AddAsync(pedido, ct);

    public async Task UpdatePedidoAsync(PedidoCompra p, CancellationToken ct = default)
    {
        // Desanexar para evitar conflitos do change tracker
        _ctx.ChangeTracker.Clear();

        await _ctx.Database.ExecuteSqlInterpolatedAsync($@"
            UPDATE ""PedidosCompra"" SET
                ""ObraId"" = {p.ObraId},
                ""FornecedorId"" = {p.FornecedorId},
                ""AlmoxarifadoId"" = {p.AlmoxarifadoId},
                ""DataEntregaPrevista"" = {p.DataEntregaPrevista},
                ""FormaPagamento"" = {p.FormaPagamento},
                ""CondicoesPagamento"" = {p.CondicoesPagamento},
                ""LocalEntrega"" = {p.LocalEntrega},
                ""ValorFrete"" = {p.ValorFrete},
                ""ValorDesconto"" = {p.ValorDesconto},
                ""Observacoes"" = {p.Observacoes},
                ""UpdatedAt"" = {DateTime.UtcNow}
            WHERE ""Id"" = {p.Id}", ct);
    }

    public async Task ReplaceItensPedidoAsync(Guid pedidoId, Guid empresaId, List<ItemPedidoCompra> novosItens, CancellationToken ct = default)
    {
        // Hard delete dos itens antigos via SQL
        await _ctx.Database.ExecuteSqlInterpolatedAsync(
            $"DELETE FROM \"ItensPedidoCompra\" WHERE \"PedidoId\" = {pedidoId}", ct);

        // Inserir novos itens
        if (novosItens.Count > 0)
        {
            await _ctx.ItensPedidoCompra.AddRangeAsync(novosItens, ct);
            await _ctx.SaveChangesAsync(ct);
        }

        // Recalcular valor total do pedido
        var valorTotal = novosItens.Sum(i => i.QuantidadePedida * i.PrecoUnitario);
        await _ctx.Database.ExecuteSqlInterpolatedAsync(
            $"UPDATE \"PedidosCompra\" SET \"ValorTotal\" = {valorTotal} WHERE \"Id\" = {pedidoId}", ct);
    }

    public async Task RemoveItensPedidoAsync(Guid pedidoId, IEnumerable<ItemPedidoCompra> trackedItens, CancellationToken ct = default)
    {
        await _ctx.Database.ExecuteSqlInterpolatedAsync(
            $"DELETE FROM \"ItensPedidoCompra\" WHERE \"PedidoId\" = {pedidoId}", ct);

        foreach (var item in trackedItens)
            _ctx.Entry(item).State = EntityState.Detached;
    }

    public async Task<IEnumerable<Cotacao>> GetCotacoesAsync(Guid empresaId, Guid? obraId, CancellationToken ct = default)
    {
        var q = _ctx.Cotacoes.Include(c => c.Obra).Include(c => c.FornecedoresConvidados).Include(c => c.Propostas).Where(c => c.EmpresaId == empresaId && !c.IsDeleted);
        if (obraId.HasValue) q = q.Where(c => c.ObraId == obraId);
        return await q.OrderByDescending(c => c.DataAbertura).Take(50).ToListAsync(ct);
    }

    public async Task<Cotacao?> GetCotacaoByIdAsync(Guid id, Guid empresaId, CancellationToken ct = default)
        => await _ctx.Cotacoes.Include(c => c.Obra).Include(c => c.Itens).Include(c => c.FornecedoresConvidados).Include(c => c.Propostas).FirstOrDefaultAsync(c => c.Id == id && c.EmpresaId == empresaId, ct);

    public async Task AddCotacaoAsync(Cotacao cotacao, CancellationToken ct = default)
        => await _ctx.Cotacoes.AddAsync(cotacao, ct);

    public async Task RemoveItensCotacaoAsync(Guid cotacaoId, CancellationToken ct = default)
        => await _ctx.Database.ExecuteSqlInterpolatedAsync(
            $"DELETE FROM \"ItensCotacao\" WHERE \"CotacaoId\" = {cotacaoId}", ct);

    public async Task ReplaceItensCotacaoAsync(Guid cotacaoId, List<ItemCotacao> novosItens, CancellationToken ct = default)
    {
        // Deletar itens antigos via SQL
        await _ctx.Database.ExecuteSqlInterpolatedAsync(
            $"DELETE FROM \"ItensCotacao\" WHERE \"CotacaoId\" = {cotacaoId}", ct);

        // Limpar change tracker para evitar conflito de concorrência
        _ctx.ChangeTracker.Clear();

        // Inserir novos itens
        if (novosItens.Count > 0)
        {
            await _ctx.ItensCotacao.AddRangeAsync(novosItens, ct);
            await _ctx.SaveChangesAsync(ct);
        }
    }

    // ─── Fornecedores Convidados ─────────────────────────────────────────────────

    public async Task<IEnumerable<FornecedorCotacao>> GetFornecedoresCotacaoAsync(Guid cotacaoId, Guid empresaId, CancellationToken ct = default)
        => await _ctx.FornecedoresCotacao
            .Include(fc => fc.Fornecedor)
            .Where(fc => fc.CotacaoId == cotacaoId && fc.EmpresaId == empresaId && !fc.IsDeleted)
            .OrderBy(fc => fc.ConvidadoEm)
            .ToListAsync(ct);

    public async Task<FornecedorCotacao?> GetFornecedorCotacaoAsync(Guid cotacaoId, Guid fornecedorId, Guid empresaId, CancellationToken ct = default)
        => await _ctx.FornecedoresCotacao
            .FirstOrDefaultAsync(fc => fc.CotacaoId == cotacaoId && fc.FornecedorId == fornecedorId && fc.EmpresaId == empresaId && !fc.IsDeleted, ct);

    public async Task AddFornecedorCotacaoAsync(FornecedorCotacao fc, CancellationToken ct = default)
        => await _ctx.FornecedoresCotacao.AddAsync(fc, ct);

    // ─── Propostas ───────────────────────────────────────────────────────────────

    public async Task<IEnumerable<PropostaCotacao>> GetPropostasCotacaoAsync(Guid cotacaoId, Guid empresaId, CancellationToken ct = default)
        => await _ctx.PropostasCotacao
            .Include(p => p.Fornecedor)
            .Include(p => p.Itens).ThenInclude(i => i.ItemCotacao)
            .Where(p => p.CotacaoId == cotacaoId && p.EmpresaId == empresaId && !p.IsDeleted)
            .OrderBy(p => p.DataRecebimento)
            .ToListAsync(ct);

    public async Task<PropostaCotacao?> GetPropostaByIdAsync(Guid propostaId, Guid empresaId, CancellationToken ct = default)
        => await _ctx.PropostasCotacao
            .Include(p => p.Fornecedor)
            .Include(p => p.Itens).ThenInclude(i => i.ItemCotacao)
            .FirstOrDefaultAsync(p => p.Id == propostaId && p.EmpresaId == empresaId && !p.IsDeleted, ct);

    public async Task AddPropostaAsync(PropostaCotacao proposta, CancellationToken ct = default)
        => await _ctx.PropostasCotacao.AddAsync(proposta, ct);

    public async Task ReplaceItensPropostaAsync(Guid propostaId, List<ItemPropostaCotacao> novosItens, CancellationToken ct = default)
    {
        await _ctx.Database.ExecuteSqlInterpolatedAsync(
            $"DELETE FROM \"ItensPropostaCotacao\" WHERE \"PropostaId\" = {propostaId}", ct);
        _ctx.ChangeTracker.Clear();
        if (novosItens.Count > 0)
        {
            await _ctx.ItensPropostaCotacao.AddRangeAsync(novosItens, ct);
            await _ctx.SaveChangesAsync(ct);
        }
    }
}
