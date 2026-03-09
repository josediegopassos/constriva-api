using Constriva.Domain.Entities.Common;

namespace Constriva.Domain.Entities.GED;

public class DocumentoTransmittal : TenantEntity
{
    public Guid TransmittalId { get; set; }
    public Guid DocumentoId { get; set; }
    public string? Finalidade { get; set; }
    public virtual Transmittal Transmittal { get; set; } = null!;
    public virtual DocumentoGED Documento { get; set; } = null!;
}
