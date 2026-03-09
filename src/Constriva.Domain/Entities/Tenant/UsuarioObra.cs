using Constriva.Domain.Entities.Common;

namespace Constriva.Domain.Entities.Tenant;

public class UsuarioObra : BaseEntity
{
    public Guid UsuarioId { get; set; }
    public Guid ObraId { get; set; }
    public Guid EmpresaId { get; set; }
    public string Funcao { get; set; } = null!;
    public bool Ativo { get; set; } = true;
    public virtual Usuario Usuario { get; set; } = null!;
}
