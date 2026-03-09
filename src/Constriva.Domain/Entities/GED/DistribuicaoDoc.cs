using Constriva.Domain.Entities.Common;

namespace Constriva.Domain.Entities.GED;

public class DistribuicaoDoc : TenantEntity  // Transmittal
{
    public Guid DocumentoId { get; set; }
    public string? DestinatarioNome { get; set; }
    public string? DestinatarioEmail { get; set; }
    public Guid? DestinatarioId { get; set; }
    public DateTime DataDistribuicao { get; set; }
    public string? Finalidade { get; set; }  // Aprovacao, Informacao, Construir
    public bool Confirmado { get; set; } = false;
    public DateTime? DataConfirmacao { get; set; }
    public virtual DocumentoGED Documento { get; set; } = null!;
}
