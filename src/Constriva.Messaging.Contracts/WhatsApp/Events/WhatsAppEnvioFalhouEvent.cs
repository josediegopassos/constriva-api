using Constriva.Messaging.Contracts.Base;

namespace Constriva.Messaging.Contracts.WhatsApp.Events;

public record WhatsAppEnvioFalhouEvent : IEvent
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public Guid CorrelacaoId { get; init; } = Guid.NewGuid();
    public DateTime CriadoEm { get; init; } = DateTime.UtcNow;
    public string Origem { get; init; } = "Constriva.Messaging";

    public Guid CotacaoId { get; init; }
    public Guid FornecedorCotacaoId { get; init; }
    public Guid FornecedorId { get; init; }
    public Guid EmpresaId { get; init; }
    public string NomeFornecedor { get; init; } = string.Empty;
    public string Erro { get; init; } = string.Empty;
    public DateTime FalhouEm { get; init; }
}
