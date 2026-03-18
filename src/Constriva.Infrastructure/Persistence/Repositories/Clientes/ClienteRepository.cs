using Microsoft.EntityFrameworkCore;
using Constriva.Domain.Entities.Clientes;
using Constriva.Domain.Enums;
using Constriva.Domain.Interfaces.Repositories;

namespace Constriva.Infrastructure.Persistence.Repositories.Clientes;

public class ClienteRepository : TenantRepository<Cliente>, IClienteRepository
{
    public ClienteRepository(AppDbContext ctx) : base(ctx) { }

    public async Task<Cliente?> GetByIdAndEmpresaAsync(Guid id, Guid empresaId, CancellationToken ct = default)
        => await _ctx.Clientes
            .FirstOrDefaultAsync(c => c.Id == id && c.EmpresaId == empresaId && !c.IsDeleted, ct);

    public async Task<(IEnumerable<Cliente> Items, int Total)> GetPagedAsync(
        Guid empresaId, string? search, StatusClienteEnum? status, int page, int pageSize, CancellationToken ct = default)
    {
        var q = _ctx.Clientes.Where(c => c.EmpresaId == empresaId && !c.IsDeleted);

        if (!string.IsNullOrWhiteSpace(search))
            q = q.Where(c => c.Nome.Contains(search) ||
                              (c.NomeFantasia != null && c.NomeFantasia.Contains(search)) ||
                              (c.Documento != null && c.Documento.Contains(search)) ||
                              (c.Email != null && c.Email.Contains(search)));

        if (status.HasValue)
            q = q.Where(c => c.Status == status.Value);

        var total = await q.CountAsync(ct);
        var items = await q.OrderBy(c => c.Nome)
            .Skip((page - 1) * pageSize).Take(pageSize).ToListAsync(ct);

        return (items, total);
    }

    public async Task<int> GetCountByEmpresaAsync(Guid empresaId, CancellationToken ct = default)
        => await _ctx.Clientes.CountAsync(c => c.EmpresaId == empresaId, ct);

    public async Task<bool> DocumentoExistsAsync(string documento, Guid empresaId, Guid? excludeId, CancellationToken ct = default)
        => await _ctx.Clientes.AnyAsync(c =>
            c.Documento == documento &&
            c.EmpresaId == empresaId &&
            !c.IsDeleted &&
            (excludeId == null || c.Id != excludeId), ct);
}
