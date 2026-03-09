using Constriva.Domain.Entities.Common;

namespace Constriva.Domain.Entities.GED;

public class FluxoAprovacaoDoc : TenantEntity
{
    public Guid DocumentoId { get; set; }
    public Guid AprovadorId { get; set; }
    public int Ordem { get; set; }
    public string Status { get; set; } = "Pendente";  // Pendente, Aprovado, Rejeitado
    public string? Comentario { get; set; }
    public DateTime? DataAprovacao { get; set; }
    public virtual DocumentoGED Documento { get; set; } = null!;
}
