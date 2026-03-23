using Constriva.Domain.Entities.Common;

namespace Constriva.Domain.Entities.Agente;

public class AgenteCotaAvulsa : TenantEntity
{
    public long TokensConcedidos { get; set; }
    public long TokensUtilizados { get; set; }
    public long TokensRestantes => TokensConcedidos - TokensUtilizados;
    public string? Motivo { get; set; }
    public DateTime ConcedidoEm { get; set; } = DateTime.UtcNow;
    public DateTime? ExpiraEm { get; set; }
    public Guid ConcedidoPorUsuarioId { get; set; }
}
