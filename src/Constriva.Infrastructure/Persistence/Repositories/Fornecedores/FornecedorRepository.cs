using Microsoft.EntityFrameworkCore;
using Constriva.Domain.Entities.Fornecedores;
using Constriva.Domain.Enums;
using Constriva.Domain.Interfaces.Repositories;

namespace Constriva.Infrastructure.Persistence.Repositories;

public class FornecedorRepository : TenantRepository<Fornecedor>, IFornecedorRepository
{
    public FornecedorRepository(AppDbContext ctx) : base(ctx) { }

    public async Task<Fornecedor?> GetByIdComEnderecoAsync(Guid id, Guid empresaId, CancellationToken ct = default)
        => await _set.Include(f => f.Endereco)
            .FirstOrDefaultAsync(f => f.Id == id && f.EmpresaId == empresaId, ct);

    public async Task<Fornecedor?> GetByDocumentoAsync(Guid empresaId, string documento, CancellationToken ct = default)
        => await _set.FirstOrDefaultAsync(f => f.EmpresaId == empresaId && f.Documento == documento, ct);

    public async Task<IEnumerable<Fornecedor>> SearchAsync(Guid empresaId, string termo, CancellationToken ct = default)
        => await _set.Where(f => f.EmpresaId == empresaId && !f.IsDeleted &&
            (f.RazaoSocial.Contains(termo) || f.Documento.Contains(termo) || (f.NomeFantasia != null && f.NomeFantasia.Contains(termo))))
            .Take(50).ToListAsync(ct);

    public async Task<(IEnumerable<Fornecedor> Items, int Total)> GetPagedAsync(
        Guid empresaId, string? search, TipoFornecedorEnum? tipo, int page, int pageSize, CancellationToken ct = default)
    {
        var q = _set.Include(f => f.Endereco).Where(f => f.EmpresaId == empresaId && !f.IsDeleted);
        if (!string.IsNullOrWhiteSpace(search))
            q = q.Where(f => f.RazaoSocial.Contains(search) || (f.NomeFantasia != null && f.NomeFantasia.Contains(search)) || f.Documento.Contains(search));
        if (tipo.HasValue) q = q.Where(f => f.Tipo == tipo);
        var total = await q.CountAsync(ct);
        var items = await q.OrderBy(f => f.RazaoSocial).Skip((page - 1) * pageSize).Take(pageSize).ToListAsync(ct);
        return (items, total);
    }
}
