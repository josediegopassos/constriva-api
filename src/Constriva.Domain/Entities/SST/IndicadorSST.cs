using Constriva.Domain.Entities.Common;

namespace Constriva.Domain.Entities.SST;

public class IndicadorSST : TenantEntity
{
    public Guid ObraId { get; set; }
    public string Competencia { get; set; } = null!;  // YYYY/MM
    public int TotalFuncionarios { get; set; }
    public int TotalHHT { get; set; }  // Homem-Hora Trabalhado
    public int AcidentesComAfastamento { get; set; }
    public int AcidentesSemAfastamento { get; set; }
    public int QuaseAcidentes { get; set; }
    public int DiasAfastados { get; set; }
    public int NumeroDDS { get; set; }
    public int DiasUteis { get; set; }

    public decimal TaxaFrequencia => TotalHHT == 0 ? 0 : (AcidentesComAfastamento * 1000000m) / TotalHHT;
    public decimal TaxaGravidade => TotalHHT == 0 ? 0 : (DiasAfastados * 1000000m) / TotalHHT;
}
