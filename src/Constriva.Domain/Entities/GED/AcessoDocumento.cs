using Constriva.Domain.Entities.Common;

namespace Constriva.Domain.Entities.GED;

public class AcessoDocumento : TenantEntity
{
    public Guid DocumentoId { get; set; }
    public Guid UsuarioId { get; set; }
    public bool PodeVisualizar { get; set; } = true;
    public bool PodeEditar { get; set; } = false;
    public bool PodeBaixar { get; set; } = true;
    public virtual DocumentoGED Documento { get; set; } = null!;
}
