using Constriva.Domain.Entities.Common;

namespace Constriva.Domain.Entities.Agente;

public class AgenteEmpresaConfig : TenantEntity
{
    public Guid AgenteTierId { get; set; }
    public bool Ativo { get; set; } = true;
    public DateTime? DataAtivacao { get; set; }
    public DateTime? DataDesativacao { get; set; }

    public virtual AgenteTier Tier { get; set; } = null!;
}
