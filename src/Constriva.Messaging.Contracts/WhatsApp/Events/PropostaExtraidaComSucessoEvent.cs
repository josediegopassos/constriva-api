using Constriva.Messaging.Contracts.Base;

namespace Constriva.Messaging.Contracts.WhatsApp.Events;

public record PropostaExtraidaComSucessoEvent : IEvent
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public Guid CorrelacaoId { get; init; } = Guid.NewGuid();
    public DateTime CriadoEm { get; init; } = DateTime.UtcNow;
    public string Origem { get; init; } = "Constriva.Messaging";

    public Guid CotacaoId { get; init; }
    public Guid FornecedorCotacaoId { get; init; }
    public Guid FornecedorId { get; init; }
    public Guid EmpresaId { get; init; }
    public string WaMessageId { get; init; } = string.Empty;
    public DateTime ExtraidaEm { get; init; }
    public int NivelConfianca { get; init; }
    public string? CondicoesPagamento { get; init; }
    public int? PrazoEntregaDias { get; init; }
    public DateTime? ValidadeProposta { get; init; }
    public string? Observacoes { get; init; }
    public IReadOnlyList<ItemPropostaExtraidoDto> ItensExtraidos { get; init; } = [];
}

public record ItemPropostaExtraidoDto
{
    public Guid ItemCotacaoId { get; init; }
    public string DescricaoOriginal { get; init; } = string.Empty;
    public decimal PrecoUnitario { get; init; }
    public decimal Quantidade { get; init; }
    public string? Marca { get; init; }
    public bool Disponivel { get; init; }
    public string? Observacao { get; init; }
}
