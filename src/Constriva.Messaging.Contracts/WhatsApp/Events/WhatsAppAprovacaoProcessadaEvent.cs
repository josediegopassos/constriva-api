using Constriva.Messaging.Contracts.Base;

namespace Constriva.Messaging.Contracts.WhatsApp.Events;

public record WhatsAppAprovacaoProcessadaEvent : IEvent
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public Guid CorrelacaoId { get; init; } = Guid.NewGuid();
    public DateTime CriadoEm { get; init; } = DateTime.UtcNow;
    public string Origem { get; init; } = "Constriva.Messaging";

    public Guid CotacaoId { get; init; }
    public Guid PropostaCotacaoId { get; init; }
    public Guid EmpresaId { get; init; }
    public string NomeFornecedorVencedor { get; init; } = string.Empty;
    public decimal ValorTotalAprovado { get; init; }
    public string NumeroPedidoCompra { get; init; } = string.Empty;
    public DateTime AprovadaEm { get; init; }
}
