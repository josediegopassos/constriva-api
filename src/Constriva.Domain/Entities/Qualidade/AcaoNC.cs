using Constriva.Domain.Entities.Common;

namespace Constriva.Domain.Entities.Qualidade;

public class AcaoNC : TenantEntity
{
    public Guid NaoConformidadeId { get; set; }
    public string Descricao { get; set; } = null!;
    public string Tipo { get; set; } = null!;  // Corretiva, Preventiva
    public Guid? ResponsavelId { get; set; }
    public DateTime? PrazoConclusao { get; set; }
    public DateTime? DataConclusao { get; set; }
    public string? Evidencia { get; set; }
    public bool Concluida { get; set; } = false;
    public virtual NaoConformidade NaoConformidade { get; set; } = null!;
}
