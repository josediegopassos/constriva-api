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

// ─── RH Repository ────────────────────────────────────────────────────────────
public class RHRepository : IRHRepository
{
    private readonly AppDbContext _ctx;
    public RHRepository(AppDbContext ctx) => _ctx = ctx;

    public async Task<(IEnumerable<Funcionario> Items, int Total)> GetFuncionariosPagedAsync(
        Guid empresaId, string? search, Guid? obraId, StatusFuncionarioEnum? status, int page, int pageSize, CancellationToken ct = default)
    {
        var q = _ctx.Funcionarios.Include(f => f.Cargo).Include(f => f.Departamento)
            .Where(f => f.EmpresaId == empresaId && !f.IsDeleted);
        if (!string.IsNullOrWhiteSpace(search))
            q = q.Where(f => f.Nome.Contains(search) || f.Matricula.Contains(search) || f.Cpf.Contains(search));
        if (obraId.HasValue) q = q.Where(f => f.ObraAtualId == obraId);
        if (status.HasValue) q = q.Where(f => f.Status == status);
        var total = await q.CountAsync(ct);
        var items = await q.OrderBy(f => f.Nome).Skip((page - 1) * pageSize).Take(pageSize).ToListAsync(ct);
        return (items, total);
    }

    public async Task<Funcionario?> GetFuncionarioByIdAsync(Guid id, Guid empresaId, CancellationToken ct = default)
        => await _ctx.Funcionarios.Include(f => f.Cargo).Include(f => f.Departamento)
            .FirstOrDefaultAsync(f => f.Id == id && f.EmpresaId == empresaId, ct);

    public async Task AddFuncionarioAsync(Funcionario func, CancellationToken ct = default) => await _ctx.Funcionarios.AddAsync(func, ct);

    public async Task<bool> CpfExisteAsync(Guid empresaId, string cpf, CancellationToken ct = default)
        => await _ctx.Funcionarios.AnyAsync(f => f.EmpresaId == empresaId && f.Cpf == cpf && !f.IsDeleted, ct);

    public async Task<int> CountFuncionariosAsync(Guid empresaId, CancellationToken ct = default)
        => await _ctx.Funcionarios.CountAsync(f => f.EmpresaId == empresaId && !f.IsDeleted, ct);

    public async Task<IEnumerable<RegistroPonto>> GetPontosAsync(Guid empresaId, Guid? funcionarioId, DateTime? inicio, DateTime? fim, CancellationToken ct = default)
    {
        var q = _ctx.RegistrosPonto.Where(p => p.EmpresaId == empresaId && !p.IsDeleted);
        if (funcionarioId.HasValue) q = q.Where(p => p.FuncionarioId == funcionarioId);
        if (inicio.HasValue) q = q.Where(p => p.DataHora >= inicio);
        if (fim.HasValue) q = q.Where(p => p.DataHora <= fim);
        return await q.OrderByDescending(p => p.DataHora).Take(200).ToListAsync(ct);
    }

    public async Task AddPontoAsync(RegistroPonto ponto, CancellationToken ct = default) => await _ctx.RegistrosPonto.AddAsync(ponto, ct);

    public async Task<RegistroPonto?> GetPontoByIdAsync(Guid id, Guid empresaId, CancellationToken ct = default)
        => await _ctx.RegistrosPonto.FirstOrDefaultAsync(p => p.Id == id && p.EmpresaId == empresaId && !p.IsDeleted, ct);

    public async Task<IEnumerable<FolhaPagamento>> GetFolhasAsync(Guid empresaId, string? competencia, CancellationToken ct = default)
    {
        var q = _ctx.FolhasPagamento.Where(f => f.EmpresaId == empresaId && !f.IsDeleted);
        if (!string.IsNullOrWhiteSpace(competencia)) q = q.Where(f => f.Competencia == competencia);
        return await q.OrderByDescending(f => f.Competencia).Take(24).ToListAsync(ct);
    }

    public async Task<IEnumerable<Cargo>> GetCargosAsync(Guid empresaId, CancellationToken ct = default)
        => await _ctx.Cargos.Where(c => c.EmpresaId == empresaId && !c.IsDeleted).OrderBy(c => c.Nome).ToListAsync(ct);

    public async Task<IEnumerable<Departamento>> GetDepartamentosAsync(Guid empresaId, CancellationToken ct = default)
        => await _ctx.Departamentos.Where(d => d.EmpresaId == empresaId && !d.IsDeleted).OrderBy(d => d.Nome).ToListAsync(ct);

    public async Task<FolhaPagamento> GerarFolhaAsync(Guid empresaId, string competencia, Guid? funcionarioId, CancellationToken ct = default)
    {
        var qFuncionarios = _ctx.Funcionarios.Where(f => f.EmpresaId == empresaId && !f.IsDeleted && f.Status == StatusFuncionarioEnum.Ativo);
        if (funcionarioId.HasValue) qFuncionarios = qFuncionarios.Where(f => f.Id == funcionarioId);
        var funcionarios = await qFuncionarios.Include(f => f.Cargo).ToListAsync(ct);

        var folha = new FolhaPagamento
        {
            EmpresaId = empresaId,
            Competencia = competencia,
            ValorTotalBruto = funcionarios.Sum(f => f.Cargo != null ? f.Cargo.SalarioBase : 0),
            ValorTotalDescontos = 0,
            ValorTotalLiquido = funcionarios.Sum(f => f.Cargo != null ? f.Cargo.SalarioBase : 0)
        };
        await _ctx.FolhasPagamento.AddAsync(folha, ct);
        return folha;
    }
}
