using Constriva.Domain.Entities.Common;

namespace Constriva.Domain.Entities.Qualidade;

public class FotoInspecao : TenantEntity
{
    public Guid InspecaoId { get; set; }
    public string Url { get; set; } = null!;
    public string? Legenda { get; set; }
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }
    public DateTime DataCaptura { get; set; }
    public virtual Inspecao Inspecao { get; set; } = null!;
}
