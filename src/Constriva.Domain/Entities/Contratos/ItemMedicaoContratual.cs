using Constriva.Domain.Entities.Common;

namespace Constriva.Domain.Entities.Contratos;

public class ItemMedicaoContratual : TenantEntity
{
    public Guid MedicaoId { get; set; }
    public string Descricao { get; set; } = null!;
    public string UnidadeMedida { get; set; } = null!;
    public decimal QuantidadeContratada { get; set; }
    public decimal QuantidadeAnterior { get; set; }
    public decimal QuantidadeAtual { get; set; }
    public decimal QuantidadeAcumulada => QuantidadeAnterior + QuantidadeAtual;
    public decimal PrecoUnitario { get; set; }
    public decimal ValorAtual => QuantidadeAtual * PrecoUnitario;
    public virtual MedicaoContratual Medicao { get; set; } = null!;
}
