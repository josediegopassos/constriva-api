namespace Constriva.Application.Features.Financeiro.DTOs;

/// <summary>
/// Parâmetros de cálculo da DRE. Configurável via appsettings.json (seção "DRE").
/// Valores padrão são referências do setor de construção civil.
/// </summary>
public class DREConfig
{
    public const string Section = "DRE";

    /// <summary>Percentual das despesas totais alocado em Custos Diretos (ex.: mão de obra, materiais).</summary>
    public decimal PercentualCustosDiretos { get; set; } = 0.70m;

    /// <summary>Percentual das despesas totais alocado em Despesas Operacionais (ex.: administrativo, comercial).</summary>
    public decimal PercentualDespesasOperacionais { get; set; } = 0.30m;

    /// <summary>Percentual do EBITDA alocado em Depreciação e Amortização.</summary>
    public decimal PercentualDepreciacao { get; set; } = 0.05m;

    /// <summary>Alíquota de Imposto de Renda sobre o Lucro Antes do IR.</summary>
    public decimal AliquotaIR { get; set; } = 0.15m;
}
