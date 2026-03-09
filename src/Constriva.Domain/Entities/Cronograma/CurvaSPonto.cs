using Constriva.Domain.Entities.Common;

namespace Constriva.Domain.Entities.Cronograma;

public class CurvaSPonto : TenantEntity
{
    public Guid CronogramaId { get; set; }
    public DateTime DataReferencia { get; set; }
    public decimal PercentualPrevisto { get; set; }
    public decimal PercentualRealizado { get; set; }
    public decimal ValorPrevisto { get; set; }
    public decimal ValorRealizado { get; set; }
    public virtual CronogramaObra Cronograma { get; set; } = null!;
}
