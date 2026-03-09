using Constriva.Domain.Entities.Common;

namespace Constriva.Domain.Entities.GED;

public class Transmittal : TenantEntity
{
    public Guid? ObraId { get; set; }
    public string Numero { get; set; } = null!;
    public string Assunto { get; set; } = null!;
    public string? Observacoes { get; set; }
    public DateTime DataEnvio { get; set; }
    public string? RemetNome { get; set; }
    public string? DestNome { get; set; }
    public string? DestEmail { get; set; }
    public bool Respondido { get; set; } = false;
    public DateTime? DataResposta { get; set; }
    public Guid CriadoPor { get; set; }

    public virtual ICollection<DocumentoTransmittal> Documentos { get; set; } = new List<DocumentoTransmittal>();
}
