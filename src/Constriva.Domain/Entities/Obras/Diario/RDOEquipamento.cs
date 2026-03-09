using Constriva.Domain.Entities.Common;

namespace Constriva.Domain.Entities.Obras.Diario;

public class RDOEquipamento : TenantEntity
{
    public Guid RDOId { get; set; }
    public string Descricao { get; set; } = null!;
    public int Quantidade { get; set; }
    public decimal HorasUtilizadas { get; set; }
    public string? Estado { get; set; }
    public virtual RDO RDO { get; set; } = null!;
}
