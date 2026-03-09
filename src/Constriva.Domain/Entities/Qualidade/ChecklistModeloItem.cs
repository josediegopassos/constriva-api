using Constriva.Domain.Entities.Common;

namespace Constriva.Domain.Entities.Qualidade;

public class ChecklistModeloItem : TenantEntity
{
    public Guid ModeloId { get; set; }
    public Guid? GrupoId { get; set; }
    public string Descricao { get; set; } = null!;
    public string? Criterio { get; set; }
    public bool Obrigatorio { get; set; } = true;
    public string TipoResposta { get; set; } = "SimNao";  // SimNao, Numerico, Texto, Foto
    public int Ordem { get; set; }
    public string? NormaReferencia { get; set; }
    public virtual ChecklistModelo Modelo { get; set; } = null!;
}
