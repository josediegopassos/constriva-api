using Constriva.Domain.Entities.Common;

namespace Constriva.Domain.Entities.RH;

public class FolhaFuncionario : TenantEntity
{
    public Guid FolhaId { get; set; }
    public Guid FuncionarioId { get; set; }
    public decimal SalarioBruto { get; set; }
    public decimal HorasExtras { get; set; }
    public decimal ValorHorasExtras { get; set; }
    public decimal AdicionalNoturno { get; set; }
    public decimal Periculosidade { get; set; }
    public decimal Insalubridade { get; set; }
    public decimal OutrasVerbas { get; set; }
    public decimal TotalProventos { get; set; }
    public decimal INSS { get; set; }
    public decimal IRRF { get; set; }
    public decimal ValeTransporte { get; set; }
    public decimal ValeRefeicao { get; set; }
    public decimal OutrosDescontos { get; set; }
    public decimal TotalDescontos { get; set; }
    public decimal SalarioLiquido { get; set; }
    public decimal FGTS { get; set; }

    public virtual FolhaPagamento Folha { get; set; } = null!;
    public virtual Funcionario Funcionario { get; set; } = null!;
}
