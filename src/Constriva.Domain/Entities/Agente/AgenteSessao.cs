using Constriva.Domain.Entities.Common;

namespace Constriva.Domain.Entities.Agente;

public class AgenteSessao : TenantEntity
{
    public Guid UsuarioId { get; set; }
    public DateTime AtualizadaEm { get; set; } = DateTime.UtcNow;
    public bool Ativa { get; set; } = true;

    public virtual ICollection<AgenteHistorico> Mensagens { get; set; } = new List<AgenteHistorico>();
}
