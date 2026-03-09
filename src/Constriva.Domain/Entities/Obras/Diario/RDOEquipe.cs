using Constriva.Domain.Entities.Common;

namespace Constriva.Domain.Entities.Obras.Diario;

public class RDOEquipe : TenantEntity
{
    public Guid RDOId { get; set; }
    public string Funcao { get; set; } = null!;
    public int Quantidade { get; set; }
    public decimal HorasTrabalhadas { get; set; }
    public string? Empresa { get; set; }
    public virtual RDO RDO { get; set; } = null!;
}
