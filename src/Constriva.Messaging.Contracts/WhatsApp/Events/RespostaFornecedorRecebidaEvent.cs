using Constriva.Messaging.Contracts.WhatsApp.Commands;
using Constriva.Messaging.Contracts.Base;

namespace Constriva.Messaging.Contracts.WhatsApp.Events;

public record RespostaFornecedorRecebidaEvent : IEvent
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
    public DateTime RecebidaEm { get; init; }
    public TipoConteudoWhatsApp TipoConteudo { get; init; }
    public string WaMessageId { get; init; } = string.Empty;
}
