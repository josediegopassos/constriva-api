using MediatR;
using Microsoft.Extensions.Options;
using Constriva.Application.Common.Behaviors;
using Constriva.Application.Common.Interfaces;
using Constriva.Application.Features.Financeiro;
using Constriva.Domain.Enums;
using Constriva.Domain.Interfaces.Repositories;
using Constriva.Application.Features.Financeiro.DTOs;

namespace Constriva.Application.Features.Financeiro.Queries;

public record GetDREQuery(Guid EmpresaId, Guid? ObraId, string? Competencia)
    : IRequest<DREDto>, ITenantRequest { public Guid TenantId => EmpresaId; }

public class GetDREHandler : IRequestHandler<GetDREQuery, DREDto>
{
    private readonly ILancamentoFinanceiroRepository _repo;
    private readonly DREConfig _config;

    public GetDREHandler(ILancamentoFinanceiroRepository repo, IOptions<DREConfig> config)
    {
        _repo = repo;
        _config = config.Value;
    }

    public async Task<DREDto> Handle(GetDREQuery r, CancellationToken ct)
    {
        var periodo = r.Competencia ?? DateTime.Now.ToString("MM/yyyy");
        var (inicio, fim) = ParseCompetencia(periodo);
        var lancamentos = await _repo.GetByPeriodoAsync(r.EmpresaId, inicio, fim, ct);

        var receitas = lancamentos
            .Where(l => l.Tipo == TipoLancamentoEnum.Receita)
            .Sum(l => l.ValorRealizado ?? l.Valor);
        var despesas = lancamentos
            .Where(l => l.Tipo == TipoLancamentoEnum.Despesa)
            .Sum(l => l.ValorRealizado ?? l.Valor);

        // Cálculo DRE com percentuais configuráveis (seção "DRE" no appsettings)
        var custosDiretos = despesas * _config.PercentualCustosDiretos;
        var lucroBruto = receitas - custosDiretos;
        var despesasOperacionais = despesas * _config.PercentualDespesasOperacionais;
        var ebitda = lucroBruto - despesasOperacionais;
        var depreciacao = ebitda * _config.PercentualDepreciacao;
        var ebit = ebitda - depreciacao;
        var lucroAnteIR = ebit; // ResultadoFinanceiro = 0 (sem operações financeiras cadastradas)
        var ir = lucroAnteIR > 0 ? lucroAnteIR * _config.AliquotaIR : 0;
        var lucroLiquido = lucroAnteIR - ir;

        return new DREDto(
            periodo,
            ReceitaBruta: receitas,
            Deducoes: 0,
            ReceitaLiquida: receitas,
            CustosDiretos: custosDiretos,
            LucroBruto: lucroBruto,
            DespesasOperacionais: despesasOperacionais,
            Ebitda: ebitda,
            Depreciacao: depreciacao,
            Ebit: ebit,
            ResultadoFinanceiro: 0,
            LucroAnteIR: lucroAnteIR,
            IR: ir,
            LucroLiquido: lucroLiquido);
    }

    static (DateTime, DateTime) ParseCompetencia(string competencia)
    {
        if (DateTime.TryParseExact(competencia, "MM/yyyy",
            System.Globalization.CultureInfo.InvariantCulture,
            System.Globalization.DateTimeStyles.None, out var d))
        {
            var inicio = new DateTime(d.Year, d.Month, 1);
            var fim = inicio.AddMonths(1).AddDays(-1);
            return (inicio, fim);
        }
        var now = DateTime.Now;
        return (new DateTime(now.Year, now.Month, 1), new DateTime(now.Year, now.Month, DateTime.DaysInMonth(now.Year, now.Month)));
    }
}
