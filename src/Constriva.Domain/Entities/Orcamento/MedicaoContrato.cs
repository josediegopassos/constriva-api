using Constriva.Domain.Entities.Common;
using Constriva.Domain.Enums;

namespace Constriva.Domain.Entities.Orcamento;

public class MedicaoContrato : TenantEntity
{
    public Guid ObraId { get; set; }
    public Guid ContratoId { get; set; }
    public string Numero { get; set; } = null!;
    public int Periodo { get; set; }
    public DateTime DataInicio { get; set; }
    public DateTime DataFim { get; set; }
    public StatusMedicaoEnum Status { get; set; } = StatusMedicaoEnum.Rascunho;
    public decimal ValorMedicao { get; set; }
    public decimal ValorAcumulado { get; set; }
    public decimal PercentualMedicao { get; set; }
    public decimal PercentualAcumulado { get; set; }
    public Guid? AprovadoPor { get; set; }
    public DateTime? DataAprovacao { get; set; }
    public string? Observacoes { get; set; }

    public virtual ICollection<ItemMedicao> Itens { get; set; } = new List<ItemMedicao>();
}
