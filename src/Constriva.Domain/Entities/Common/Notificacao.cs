using Constriva.Domain.Enums;

namespace Constriva.Domain.Entities.Common;

public class Notificacao : TenantEntity
{
    public string ModuloOrigem { get; set; } = null!;
    public TipoNotificacaoEnum Tipo { get; set; }
    public string Mensagem { get; set; } = null!;
    public Guid? ReferenciaId { get; set; }
    public string? DestinatariosJson { get; set; }
    public DateTime? Prazo { get; set; }
    public bool Lida { get; set; }
}
