namespace Constriva.Application.Features.Financeiro.DTOs;

public class DREConfig
{
    public const string Section = "DRE";

    public decimal PercentualCustosDiretos { get; set; } = 0.70m;

    public decimal PercentualDespesasOperacionais { get; set; } = 0.30m;

    public decimal PercentualDepreciacao { get; set; } = 0.05m;

    public decimal AliquotaIR { get; set; } = 0.15m;
}
