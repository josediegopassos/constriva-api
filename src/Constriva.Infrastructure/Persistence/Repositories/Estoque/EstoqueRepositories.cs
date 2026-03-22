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

// ─── Material Repository ─────────────────────────────────────────────────────
public class MaterialRepository : TenantRepository<Material>, IMaterialRepository
{
    public MaterialRepository(AppDbContext ctx) : base(ctx) { }

    public async Task<Material?> GetByCodigoAsync(Guid empresaId, string codigo, CancellationToken ct = default)
        => await _set.FirstOrDefaultAsync(m => m.EmpresaId == empresaId && m.Codigo == codigo, ct);

    public async Task<IEnumerable<Material>> SearchAsync(Guid empresaId, string termo, CancellationToken ct = default)
        => await _set.Where(m => m.EmpresaId == empresaId && !m.IsDeleted &&
            (m.Nome.Contains(termo) || m.Codigo.Contains(termo) || (m.CodigoSINAPI != null && m.CodigoSINAPI.Contains(termo))))
            .Take(50).ToListAsync(ct);

    public async Task<Material?> GetWithGrupoAsync(Guid id, Guid empresaId, CancellationToken ct = default)
        => await _set.Include(m => m.Grupo)
            .FirstOrDefaultAsync(m => m.Id == id && m.EmpresaId == empresaId && !m.IsDeleted, ct);

    public async Task<IEnumerable<Material>> GetAllComGrupoAsync(Guid empresaId, CancellationToken ct = default)
        => await _set.Include(m => m.Grupo)
            .Where(m => m.EmpresaId == empresaId && !m.IsDeleted)
            .OrderBy(m => m.Nome)
            .ToListAsync(ct);
}

// ─── Estoque Repository ───────────────────────────────────────────────────────
public class EstoqueRepository : IEstoqueRepository
{
    private readonly AppDbContext _ctx;
    public EstoqueRepository(AppDbContext ctx) => _ctx = ctx;

    public async Task<IEnumerable<EstoqueSaldo>> GetSaldosAsync(Guid empresaId, Guid? almoxarifadoId, CancellationToken ct = default)
    {
        var q = _ctx.EstoquesSaldos.Include(s => s.Material).Include(s => s.Almoxarifado)
            .Where(s => s.EmpresaId == empresaId);
        if (almoxarifadoId.HasValue) q = q.Where(s => s.AlmoxarifadoId == almoxarifadoId);
        return await q.OrderBy(s => s.Material.Nome).ToListAsync(ct);
    }

    public async Task<IEnumerable<MovimentacaoEstoque>> GetMovimentacoesAsync(Guid empresaId, Guid? almoxarifadoId, DateTime? inicio, DateTime? fim, CancellationToken ct = default)
    {
        var q = _ctx.MovimentacoesEstoque.Include(m => m.Material).Include(m => m.Almoxarifado).Where(m => m.EmpresaId == empresaId);
        if (almoxarifadoId.HasValue) q = q.Where(m => m.AlmoxarifadoId == almoxarifadoId);
        if (inicio.HasValue) q = q.Where(m => m.CreatedAt >= inicio);
        if (fim.HasValue) q = q.Where(m => m.CreatedAt <= fim);
        return await q.OrderByDescending(m => m.CreatedAt).Take(200).ToListAsync(ct);
    }

    public async Task<IEnumerable<Almoxarifado>> GetAlmoxarifadosAsync(Guid empresaId, CancellationToken ct = default)
        => await _ctx.Almoxarifados.Where(a => a.EmpresaId == empresaId && a.Ativo).OrderBy(a => a.Nome).ToListAsync(ct);

    public async Task<(IEnumerable<RequisicaoMaterial> Items, int Total)> GetRequisicoesPagedAsync(
        Guid empresaId, Guid? obraId, StatusRequisicaoEnum? status, int page, int pageSize, CancellationToken ct = default)
    {
        var q = _ctx.RequisicoesMateriis.Where(r => r.EmpresaId == empresaId && !r.IsDeleted);
        if (obraId.HasValue) q = q.Where(r => r.ObraId == obraId);
        if (status.HasValue) q = q.Where(r => r.Status == status);
        var total = await q.CountAsync(ct);
        var items = await q.OrderByDescending(r => r.CreatedAt).Skip((page - 1) * pageSize).Take(pageSize).ToListAsync(ct);
        return (items, total);
    }

    public async Task<RequisicaoMaterial?> GetRequisicaoByIdAsync(Guid id, Guid empresaId, CancellationToken ct = default)
        => await _ctx.RequisicoesMateriis
            .Include(r => r.Itens).ThenInclude(i => i.Material)
            .FirstOrDefaultAsync(r => r.Id == id && r.EmpresaId == empresaId, ct);

    public async Task AddRequisicaoAsync(RequisicaoMaterial req, CancellationToken ct = default)
        => await _ctx.RequisicoesMateriis.AddAsync(req, ct);

    public async Task AddItemRequisicaoAsync(ItemRequisicao item, CancellationToken ct = default)
        => await _ctx.ItensRequisicao.AddAsync(item, ct);

    public async Task<MovimentacaoEstoque?> GetMovimentacaoByIdAsync(Guid id, Guid empresaId, CancellationToken ct = default)
        => await _ctx.MovimentacoesEstoque
            .FirstOrDefaultAsync(m => m.Id == id && m.EmpresaId == empresaId, ct);


    public async Task SoftDeleteByMaterialIdAsync(Guid materialId, Guid empresaId, CancellationToken ct = default)
    {
        var now = DateTime.UtcNow;

        var saldos = await _ctx.EstoquesSaldos
            .Where(s => s.MaterialId == materialId && s.EmpresaId == empresaId)
            .ToListAsync(ct);
        saldos.ForEach(s => s.IsDeleted = true);

        var movimentacoes = await _ctx.MovimentacoesEstoque
            .Where(m => m.MaterialId == materialId && m.EmpresaId == empresaId)
            .ToListAsync(ct);
        movimentacoes.ForEach(m => m.IsDeleted = true);

        var itensRequisicao = await _ctx.ItensRequisicao
            .Where(i => i.MaterialId == materialId && i.EmpresaId == empresaId)
            .ToListAsync(ct);
        itensRequisicao.ForEach(i => i.IsDeleted = true);

        var itensInventario = await _ctx.ItensInventario
            .Where(i => i.MaterialId == materialId && i.EmpresaId == empresaId)
            .ToListAsync(ct);
        itensInventario.ForEach(i => i.IsDeleted = true);
    }

    public async Task AddMovimentacaoAsync(MovimentacaoEstoque mov, CancellationToken ct = default)
        => await _ctx.MovimentacoesEstoque.AddAsync(mov, ct);

    public async Task<Almoxarifado?> GetAlmoxarifadoByIdAsync(Guid id, Guid empresaId, CancellationToken ct = default)
        => await _ctx.Almoxarifados.FirstOrDefaultAsync(a => a.Id == id && a.EmpresaId == empresaId, ct);

    public async Task AddAlmoxarifadoAsync(Almoxarifado almoxarifado, CancellationToken ct = default)
        => await _ctx.Almoxarifados.AddAsync(almoxarifado, ct);

    public async Task<IEnumerable<GrupoMaterial>> GetGruposAsync(Guid empresaId, CancellationToken ct = default)
        => await _ctx.GruposMateriais
            .Include(g => g.GrupoPai)
            .Where(g => g.EmpresaId == empresaId && !g.IsDeleted)
            .OrderBy(g => g.Nome)
            .ToListAsync(ct);

    public async Task<EstoqueSaldo?> GetSaldoAsync(Guid almoxarifadoId, Guid materialId, Guid empresaId, CancellationToken ct = default)
        => await _ctx.EstoquesSaldos
            .FirstOrDefaultAsync(s => s.AlmoxarifadoId == almoxarifadoId && s.MaterialId == materialId && s.EmpresaId == empresaId, ct);

    public async Task AddSaldoAsync(EstoqueSaldo saldo, CancellationToken ct = default)
        => await _ctx.EstoquesSaldos.AddAsync(saldo, ct);
}
