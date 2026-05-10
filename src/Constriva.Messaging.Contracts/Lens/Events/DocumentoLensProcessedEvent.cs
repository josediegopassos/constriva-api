using Constriva.Messaging.Contracts.Base;

namespace Constriva.Messaging.Contracts.Lens.Events;

public record DocumentoLensProcessedEvent : IEvent
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public Guid CorrelacaoId { get; init; } = Guid.NewGuid();
    public DateTime CriadoEm { get; init; } = DateTime.UtcNow;
    public string Origem { get; init; } = "Constriva.Messaging";
    public Guid ProcessamentoId { get; init; }
    public Guid UsuarioId { get; init; }
    public Guid? ObraId { get; init; }
    public Guid EmpresaId { get; init; }
    public string Status { get; init; } = string.Empty;
    public string Mensagem { get; init; } = string.Empty;
}
