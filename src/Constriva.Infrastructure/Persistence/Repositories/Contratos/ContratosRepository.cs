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

// ─── Contratos Repository ─────────────────────────────────────────────────────
public class ContratoRepository : TenantRepository<Contrato>, IContratoRepository
{
    public ContratoRepository(AppDbContext ctx) : base(ctx) { }

    public async Task<(IEnumerable<Contrato> Items, int Total)> GetPagedAsync(
        Guid empresaId, Guid? obraId, StatusContratoEnum? status, int page, int pageSize, CancellationToken ct = default)
    {
        var q = _ctx.Contratos.Include(c => c.Fornecedor)
            .Where(c => c.EmpresaId == empresaId && !c.IsDeleted);
        if (obraId.HasValue) q = q.Where(c => c.ObraId == obraId);
        if (status.HasValue) q = q.Where(c => c.Status == status);
        var total = await q.CountAsync(ct);
        var items = await q.OrderByDescending(c => c.DataAssinatura)
            .Skip((page - 1) * pageSize).Take(pageSize).ToListAsync(ct);
        return (items, total);
    }

    public async Task<int> GetCountByEmpresaAsync(Guid empresaId, CancellationToken ct = default)
        => await _ctx.Contratos.CountAsync(c => c.EmpresaId == empresaId, ct);

    public async Task<IEnumerable<MedicaoContratual>> GetMedicoesAsync(Guid contratoId, Guid empresaId, CancellationToken ct = default)
        => await _ctx.MedicoesContratuais
            .Where(m => m.ContratoId == contratoId && m.EmpresaId == empresaId && !m.IsDeleted)
            .OrderByDescending(m => m.Periodo).ToListAsync(ct);

    public async Task AddMedicaoAsync(MedicaoContratual medicao, CancellationToken ct = default)
        => await _ctx.MedicoesContratuais.AddAsync(medicao, ct);

    public async Task<decimal> GetTotalMedicoesAsync(Guid contratoId, CancellationToken ct = default)
        => await _ctx.MedicoesContratuais
            .Where(m => m.ContratoId == contratoId && !m.IsDeleted)
            .SumAsync(m => m.ValorMedicao, ct);

    public async Task<MedicaoContratual?> GetMedicaoByIdAsync(Guid id, Guid empresaId, CancellationToken ct = default)
        => await _ctx.MedicoesContratuais.FirstOrDefaultAsync(m => m.Id == id && m.EmpresaId == empresaId && !m.IsDeleted, ct);
}
