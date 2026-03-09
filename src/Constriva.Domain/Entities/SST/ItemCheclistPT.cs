using Constriva.Domain.Entities.Common;

namespace Constriva.Domain.Entities.SST;

public class ItemCheclistPT : TenantEntity
{
    public Guid PermissaoId { get; set; }
    public string Item { get; set; } = null!;
    public bool Atendido { get; set; } = false;
    public string? Observacao { get; set; }
    public virtual PermissaoTrabalho Permissao { get; set; } = null!;
}
