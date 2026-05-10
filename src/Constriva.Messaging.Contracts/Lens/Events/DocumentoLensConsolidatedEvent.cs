using Constriva.Messaging.Contracts.Base;

namespace Constriva.Messaging.Contracts.Lens.Events;

public record DocumentoLensConsolidatedEvent : IEvent
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public Guid CorrelacaoId { get; init; } = Guid.NewGuid();
    public DateTime CriadoEm { get; init; } = DateTime.UtcNow;
    public string Origem { get; init; } = "Constriva.API";
    public Guid ProcessamentoId { get; init; }
    public Guid CompraId { get; init; }
    public Guid UsuarioId { get; init; }
    public Guid? ObraId { get; init; }
    public Guid EmpresaId { get; init; }
    public int TotalItensConsolidados { get; init; }
    public int TotalItensRejeitados { get; init; }
    public decimal ValorTotal { get; init; }
}
