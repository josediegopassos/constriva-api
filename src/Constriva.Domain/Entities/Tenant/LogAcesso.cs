using Constriva.Domain.Entities.Common;

namespace Constriva.Domain.Entities.Tenant;

public class LogAcesso : BaseEntity
{
    public Guid UsuarioId { get; set; }
    public Guid? EmpresaId { get; set; }
    public string Acao { get; set; } = null!;
    public string? IpAddress { get; set; }
    public string? UserAgent { get; set; }
    public string? Detalhes { get; set; }
    public bool Sucesso { get; set; } = true;
    public virtual Usuario Usuario { get; set; } = null!;
}
