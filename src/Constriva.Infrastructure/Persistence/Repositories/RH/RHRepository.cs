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
        var q = _ctx.Funcionarios.Include(f => f.Cargo).Include(f => f.Departamento).Include(f => f.ObraAtual)
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
        => await _ctx.Funcionarios.Include(f => f.Cargo).Include(f => f.Departamento).Include(f => f.Endereco).Include(f => f.DadosBancarios).ThenInclude(d => d!.Banco).Include(f => f.ObraAtual)
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

    public async Task<Cargo?> GetCargoByIdAsync(Guid id, Guid empresaId, CancellationToken ct = default)
        => await _ctx.Cargos.FirstOrDefaultAsync(c => c.Id == id && c.EmpresaId == empresaId && !c.IsDeleted, ct);

    public async Task AddCargoAsync(Cargo cargo, CancellationToken ct = default)
        => await _ctx.Cargos.AddAsync(cargo, ct);

    public async Task<int> GetCargosCountAsync(Guid empresaId, CancellationToken ct = default)
        => await _ctx.Cargos.IgnoreQueryFilters().CountAsync(c => c.EmpresaId == empresaId, ct);

    public async Task<IEnumerable<Departamento>> GetDepartamentosAsync(Guid empresaId, CancellationToken ct = default)
        => await _ctx.Departamentos.Include(d => d.Gestor).Where(d => d.EmpresaId == empresaId && !d.IsDeleted).OrderBy(d => d.Nome).ToListAsync(ct);

    public async Task<Departamento?> GetDepartamentoByIdAsync(Guid id, Guid empresaId, CancellationToken ct = default)
        => await _ctx.Departamentos.FirstOrDefaultAsync(d => d.Id == id && d.EmpresaId == empresaId && !d.IsDeleted, ct);

    public async Task AddDepartamentoAsync(Departamento departamento, CancellationToken ct = default)
        => await _ctx.Departamentos.AddAsync(departamento, ct);

    public async Task<int> GetFuncionariosCountAsync(Guid empresaId, CancellationToken ct = default)
        => await _ctx.Funcionarios.IgnoreQueryFilters().CountAsync(f => f.EmpresaId == empresaId, ct);

    public async Task AddEnderecoAsync(Endereco endereco, CancellationToken ct = default)
        => await _ctx.Enderecos.AddAsync(endereco, ct);

    public async Task AddDadosBancariosAsync(DadosBancarios dados, CancellationToken ct = default)
        => await _ctx.DadosBancarios.AddAsync(dados, ct);

    public async Task<IEnumerable<Banco>> GetBancosAsync(CancellationToken ct = default)
        => await _ctx.Bancos.OrderBy(b => b.Codigo).ToListAsync(ct);

    public async Task<IEnumerable<Funcionario>> GetFuncionariosAtivosAsync(Guid empresaId, CancellationToken ct = default)
        => await _ctx.Funcionarios.Where(f => f.EmpresaId == empresaId && !f.IsDeleted && f.Status == StatusFuncionarioEnum.Ativo)
            .OrderBy(f => f.Nome).ToListAsync(ct);

    public async Task<FolhaPagamento> GerarFolhaAsync(Guid empresaId, string competencia, Guid? funcionarioId, CancellationToken ct = default)
    {
        var qFuncionarios = _ctx.Funcionarios.Where(f => f.EmpresaId == empresaId && !f.IsDeleted && f.Status == StatusFuncionarioEnum.Ativo);
        if (funcionarioId.HasValue) qFuncionarios = qFuncionarios.Where(f => f.Id == funcionarioId);
        var funcionarios = await qFuncionarios.Include(f => f.Cargo).ToListAsync(ct);

        // Soft delete de folhas anteriores da mesma competência
        var now = DateTime.UtcNow;
        await _ctx.Database.ExecuteSqlInterpolatedAsync(
            $"""UPDATE "FolhaFuncionarios" SET "IsDeleted" = true, "DeletedAt" = {now} WHERE "FolhaId" IN (SELECT "Id" FROM "FolhasPagamento" WHERE "EmpresaId" = {empresaId} AND "Competencia" = {competencia} AND "IsDeleted" = false)""", ct);
        await _ctx.Database.ExecuteSqlInterpolatedAsync(
            $"""UPDATE "FolhasPagamento" SET "IsDeleted" = true, "DeletedAt" = {now} WHERE "EmpresaId" = {empresaId} AND "Competencia" = {competencia} AND "IsDeleted" = false""", ct);

        var folha = new FolhaPagamento
        {
            EmpresaId = empresaId,
            Competencia = competencia,
            DataInicio = new DateTime(int.Parse(competencia[..4]), int.Parse(competencia[5..]), 1),
            DataFim = new DateTime(int.Parse(competencia[..4]), int.Parse(competencia[5..]), 1).AddMonths(1).AddDays(-1),
        };
        await _ctx.FolhasPagamento.AddAsync(folha, ct);

        foreach (var func in funcionarios)
        {
            var salarioBruto = func.SalarioBase;
            var horasExtras = 0m;
            var valorHorasExtras = 0m;
            var adicionalNoturno = 0m;
            var periculosidade = 0m;
            var insalubridade = 0m;
            var outrasVerbas = 0m;
            var totalProventos = Math.Round(salarioBruto + valorHorasExtras + adicionalNoturno + periculosidade + insalubridade + outrasVerbas, 2);

            var inss = CalcularINSS(totalProventos);
            var baseIRRF = totalProventos - inss;
            var irrf = CalcularIRRF(baseIRRF);
            var valeTransporte = Math.Round(salarioBruto * 0.06m, 2);
            var valeRefeicao = 0m;
            var outrosDescontos = 0m;
            var totalDescontos = Math.Round(inss + irrf + valeTransporte + valeRefeicao + outrosDescontos, 2);
            var salarioLiquido = Math.Round(totalProventos - totalDescontos, 2);
            var fgts = Math.Round(salarioBruto * 0.08m, 2);

            var ff = new FolhaFuncionario
            {
                EmpresaId = empresaId,
                FolhaId = folha.Id,
                FuncionarioId = func.Id,
                SalarioBruto = salarioBruto,
                HorasExtras = horasExtras,
                ValorHorasExtras = valorHorasExtras,
                AdicionalNoturno = adicionalNoturno,
                Periculosidade = periculosidade,
                Insalubridade = insalubridade,
                OutrasVerbas = outrasVerbas,
                TotalProventos = totalProventos,
                INSS = inss,
                IRRF = irrf,
                ValeTransporte = valeTransporte,
                ValeRefeicao = valeRefeicao,
                OutrosDescontos = outrosDescontos,
                TotalDescontos = totalDescontos,
                SalarioLiquido = salarioLiquido,
                FGTS = fgts,
            };
            folha.Funcionarios.Add(ff);
        }

        folha.TotalFuncionarios = funcionarios.Count;
        folha.ValorTotalBruto = Math.Round(folha.Funcionarios.Sum(f => f.TotalProventos), 2);
        folha.ValorTotalDescontos = Math.Round(folha.Funcionarios.Sum(f => f.TotalDescontos), 2);
        folha.ValorTotalLiquido = Math.Round(folha.Funcionarios.Sum(f => f.SalarioLiquido), 2);
        folha.UpdatedAt = DateTime.UtcNow;

        return folha;
    }

    public async Task<IEnumerable<FolhaFuncionario>> GetFolhasFuncionarioAsync(Guid funcionarioId, Guid empresaId, CancellationToken ct = default)
        => await _ctx.FolhasFuncionarios
            .Include(ff => ff.Folha)
            .Include(ff => ff.Funcionario)
            .Where(ff => ff.FuncionarioId == funcionarioId && ff.EmpresaId == empresaId && !ff.IsDeleted)
            .OrderByDescending(ff => ff.Folha.Competencia)
            .ToListAsync(ct);

    private static decimal CalcularINSS(decimal salario)
    {
        // INSS 2024/2025 progressive table
        const decimal faixa1Teto = 1412.00m;
        const decimal faixa2Teto = 2666.68m;
        const decimal faixa3Teto = 4000.03m;
        const decimal faixa4Teto = 7786.02m;

        var inss = 0m;

        if (salario <= faixa1Teto)
            return Math.Round(salario * 0.075m, 2);

        inss += faixa1Teto * 0.075m;

        if (salario <= faixa2Teto)
            return Math.Round(inss + (salario - faixa1Teto) * 0.09m, 2);

        inss += (faixa2Teto - faixa1Teto) * 0.09m;

        if (salario <= faixa3Teto)
            return Math.Round(inss + (salario - faixa2Teto) * 0.12m, 2);

        inss += (faixa3Teto - faixa2Teto) * 0.12m;

        var baseFaixa4 = Math.Min(salario, faixa4Teto);
        inss += (baseFaixa4 - faixa3Teto) * 0.14m;

        return Math.Round(inss, 2);
    }

    private static decimal CalcularIRRF(decimal baseCalculo)
    {
        // IRRF table (base = salario - INSS)
        if (baseCalculo <= 2259.20m)
            return 0m;
        if (baseCalculo <= 2826.65m)
            return Math.Round(baseCalculo * 0.075m - 169.44m, 2);
        if (baseCalculo <= 3751.05m)
            return Math.Round(baseCalculo * 0.15m - 381.44m, 2);
        if (baseCalculo <= 4664.68m)
            return Math.Round(baseCalculo * 0.225m - 662.77m, 2);
        return Math.Round(baseCalculo * 0.275m - 896.00m, 2);
    }
}
