using Constriva.Domain.Entities.Common;
using Constriva.Domain.Enums;

namespace Constriva.Domain.Entities.RH;

public class FolhaPagamento : TenantEntity
{
    public string Competencia { get; set; } = null!;  // YYYY/MM
    public DateTime DataInicio { get; set; }
    public DateTime DataFim { get; set; }
    public StatusFolhaPagamentoEnum Status { get; set; } = StatusFolhaPagamentoEnum.EmAberto;
    public decimal ValorTotalBruto { get; set; }
    public decimal ValorTotalDescontos { get; set; }
    public decimal ValorTotalLiquido { get; set; }
    public int TotalFuncionarios { get; set; }
    public Guid? AprovadoPor { get; set; }
    public DateTime? DataAprovacao { get; set; }
    public DateTime? DataPagamento { get; set; }

    public virtual ICollection<FolhaFuncionario> Funcionarios { get; set; } = new List<FolhaFuncionario>();
}
