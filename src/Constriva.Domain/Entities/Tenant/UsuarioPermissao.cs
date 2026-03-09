using Constriva.Domain.Entities.Common;

namespace Constriva.Domain.Entities.Tenant;

public class UsuarioPermissao : BaseEntity
{
    public Guid UsuarioId { get; set; }
    public Guid EmpresaId { get; set; }
    public string Modulo { get; set; } = null!;
    public bool PodeVisualizar { get; set; } = true;
    public bool PodeCriar { get; set; } = false;
    public bool PodeEditar { get; set; } = false;
    public bool PodeDeletar { get; set; } = false;
    public bool PodeAprovar { get; set; } = false;
    public bool PodeExportar { get; set; } = false;
    public bool PodeAdministrar { get; set; } = false;

    public virtual Usuario Usuario { get; set; } = null!;
}
