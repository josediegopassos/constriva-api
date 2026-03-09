using Constriva.Domain.Entities.Common;

namespace Constriva.Domain.Entities.Obras.Diario;

public class RDO : TenantEntity
{
    public Guid ObraId { get; set; }
    public string Numero { get; set; } = null!;
    public DateTime DataRDO { get; set; }
    public string? CondicaoTempo { get; set; }
    public string? TempoManha { get; set; }
    public string? TempoTarde { get; set; }
    public string? TempoNoite { get; set; }
    public decimal TemperaturaMin { get; set; }
    public decimal TemperaturaMax { get; set; }
    public string? AtividadesRealizadas { get; set; }
    public string? OcorrenciasObservacoes { get; set; }
    public string? PendenciasProblemas { get; set; }
    public string? Fotografias { get; set; } // JSON array de URLs
    public string? ResponsavelNome { get; set; }
    public string? ResponsavelAssinaturaUrl { get; set; }
    public Guid AutrId { get; set; }
    public bool Aprovado { get; set; } = false;
    public Guid? AprovadoPor { get; set; }
    public DateTime? DataAprovacao { get; set; }

    public virtual Obra Obra { get; set; } = null!;
    public virtual ICollection<RDOEquipe> Equipes { get; set; } = new List<RDOEquipe>();
    public virtual ICollection<RDOEquipamento> Equipamentos { get; set; } = new List<RDOEquipamento>();
    public virtual ICollection<RDOOcorrencia> Ocorrencias { get; set; } = new List<RDOOcorrencia>();
}
