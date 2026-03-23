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
        var q = _ctx.PedidosCompra.Include(p => p.Fornecedor).Where(p => p.EmpresaId == empresaId && !p.IsDeleted);
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

    public async Task<IEnumerable<Cotacao>> GetCotacoesAsync(Guid empresaId, Guid? obraId, CancellationToken ct = default)
    {
        var q = _ctx.Cotacoes.Where(c => c.EmpresaId == empresaId && !c.IsDeleted);
        if (obraId.HasValue) q = q.Where(c => c.ObraId == obraId);
        return await q.OrderByDescending(c => c.DataAbertura).Take(50).ToListAsync(ct);
    }

    public async Task AddCotacaoAsync(Cotacao cotacao, CancellationToken ct = default)
        => await _ctx.Cotacoes.AddAsync(cotacao, ct);
}
