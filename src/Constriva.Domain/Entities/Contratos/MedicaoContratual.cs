using Constriva.Domain.Entities.Common;
using Constriva.Domain.Enums;

namespace Constriva.Domain.Entities.Contratos;

public class MedicaoContratual : TenantEntity
{
    public Guid ContratoId { get; set; }
    public string Numero { get; set; } = null!;
    public int Periodo { get; set; }
    public DateTime DataInicio { get; set; }
    public DateTime DataFim { get; set; }
    public StatusMedicaoEnum Status { get; set; } = StatusMedicaoEnum.Rascunho;
    public decimal ValorMedicao { get; set; }
    public decimal ValorRetencao { get; set; }
    public decimal ValorLiquido => ValorMedicao - ValorRetencao;
    public decimal PercentualMedicao { get; set; }
    public decimal PercentualAcumulado { get; set; }
    public DateTime? DataSubmissao { get; set; }
    public Guid? AnalisadoPor { get; set; }
    public DateTime? DataAnalise { get; set; }
    public Guid? AprovadoPor { get; set; }
    public DateTime? DataAprovacao { get; set; }
    public string? MotivoRejeicao { get; set; }
    public string? Observacoes { get; set; }
    public string? ArquivoUrl { get; set; }

    public virtual Contrato Contrato { get; set; } = null!;
    public virtual ICollection<ItemMedicaoContratual> Itens { get; set; } = new List<ItemMedicaoContratual>();
}
