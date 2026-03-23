using Constriva.Domain.Entities.Common;

namespace Constriva.Domain.Entities.Agente;

public class AgenteTier : BaseEntity
{
    public string Nome { get; set; } = null!;
    public long TokensMensais { get; set; }
    public string? Descricao { get; set; }
    public bool Ativo { get; set; } = true;
}
