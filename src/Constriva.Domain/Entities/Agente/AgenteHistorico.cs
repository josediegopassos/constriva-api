using Constriva.Domain.Entities.Common;
using Constriva.Domain.Enums;

namespace Constriva.Domain.Entities.Agente;

public class AgenteHistorico : TenantEntity
{
    public Guid SessaoId { get; set; }
    public RoleAgenteEnum Role { get; set; }
    public string Conteudo { get; set; } = null!;
    public string? ToolCallsJson { get; set; }
    public int TokensInput { get; set; }
    public int TokensOutput { get; set; }

    public virtual AgenteSessao Sessao { get; set; } = null!;
}
