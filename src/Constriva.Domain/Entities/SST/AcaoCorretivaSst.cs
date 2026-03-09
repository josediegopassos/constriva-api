using Constriva.Domain.Entities.Common;

namespace Constriva.Domain.Entities.SST;

public class AcaoCorretivaSst : TenantEntity
{
    public Guid AcidenteId { get; set; }
    public string Descricao { get; set; } = null!;
    public Guid? ResponsavelId { get; set; }
    public DateTime? Prazo { get; set; }
    public bool Concluida { get; set; } = false;
    public DateTime? DataConclusao { get; set; }
    public string? Evidencia { get; set; }
    public virtual RegistroAcidente Acidente { get; set; } = null!;
}
