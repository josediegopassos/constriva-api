using Constriva.Domain.Entities.Common;
using Constriva.Domain.Enums;

namespace Constriva.Domain.Entities.SST;

public class DocumentoPCM : TenantEntity  // PCMSO, PPRA, PCMAT, etc.
{
    public Guid ObraId { get; set; }
    public string TipoDocumento { get; set; } = null!;  // PCMSO, PPRA, PCMAT, PGR, LTCAT
    public string? NumeroRevisao { get; set; }
    public DateTime DataElaboracao { get; set; }
    public DateTime? DataVigencia { get; set; }
    public StatusDocumentoSSTEnum Status { get; set; }
    public string? Elaborador { get; set; }
    public string? AprovadoPor { get; set; }
    public string? ArquivoUrl { get; set; }
    public string? Observacoes { get; set; }
}
