using Constriva.Domain.Entities.Common;

namespace Constriva.Domain.Entities.Estoque;

public class GrupoMaterial : TenantEntity
{
    public string Nome { get; set; } = null!;
    public string? Descricao { get; set; }
    public Guid? GrupoPaiId { get; set; }
    public virtual GrupoMaterial? GrupoPai { get; set; }
    public virtual ICollection<GrupoMaterial> SubGrupos { get; set; } = new List<GrupoMaterial>();
    public virtual ICollection<Material> Materiais { get; set; } = new List<Material>();
}
