using Constriva.Domain.Entities.Common;

namespace Constriva.Domain.Entities.Financeiro;

public class OrcamentoDRE : TenantEntity
{
    public Guid? ObraId { get; set; }
    public string Competencia { get; set; } = null!;
    public decimal Receitas { get; set; }
    public decimal Deducoes { get; set; }
    public decimal ReceitaLiquida => Receitas - Deducoes;
    public decimal CustosDiretos { get; set; }
    public decimal LucroBruto => ReceitaLiquida - CustosDiretos;
    public decimal DespesasAdministrativas { get; set; }
    public decimal DespesasComerciais { get; set; }
    public decimal DespesasFinanceiras { get; set; }
    public decimal ResultadoOperacional => LucroBruto - DespesasAdministrativas - DespesasComerciais;
    public decimal EBITDA => ResultadoOperacional + DespesasFinanceiras;
    public decimal IRCSLL { get; set; }
    public decimal LucroLiquido => ResultadoOperacional - DespesasFinanceiras - IRCSLL;
    public bool Realizado { get; set; } = false;
}
