using Constriva.Domain.Entities.Common;

namespace Constriva.Domain.Entities.Qualidade;

public class ItemFVS : TenantEntity
{
    public Guid FVSId { get; set; }
    public string Descricao { get; set; } = null!;
    public string? Criterio { get; set; }
    public string Resultado { get; set; } = null!;  // Conforme, NaoConforme, NA
    public string? Observacao { get; set; }
    public string? FotoUrl { get; set; }
    public virtual FVS FVSVinculado { get; set; } = null!;
}
