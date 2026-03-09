using Constriva.Domain.Entities.Common;

namespace Constriva.Domain.Entities.Obras.Diario;

public class RDOOcorrencia : TenantEntity
{
    public Guid RDOId { get; set; }
    public string Descricao { get; set; } = null!;
    public string Tipo { get; set; } = null!;
    public string? Resolucao { get; set; }
    public virtual RDO RDO { get; set; } = null!;
}
