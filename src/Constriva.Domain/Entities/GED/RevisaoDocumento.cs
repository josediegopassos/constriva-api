using Constriva.Domain.Entities.Common;

namespace Constriva.Domain.Entities.GED;

public class RevisaoDocumento : TenantEntity
{
    public Guid DocumentoId { get; set; }
    public int NumeroRevisao { get; set; }
    public string? Descricao { get; set; }
    public string Motivo { get; set; } = null!;
    public Guid RevisadoPor { get; set; }
    public DateTime DataRevisao { get; set; }
    public string? ArquivoUrl { get; set; }
    public virtual DocumentoGED Documento { get; set; } = null!;
}
