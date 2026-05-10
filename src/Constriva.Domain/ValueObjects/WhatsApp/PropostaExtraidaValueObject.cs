namespace Constriva.Domain.ValueObjects.WhatsApp;

public class PropostaExtraidaValueObject
{
    public required int NivelConfianca { get; init; }
    public string? CondicoesPagamento { get; init; }
    public int? PrazoEntregaDias { get; init; }
    public DateTime? ValidadeProposta { get; init; }
    public string? Observacoes { get; init; }
    public bool NaoEProposta { get; init; }
    public string? MotivoNaoEProposta { get; init; }
    public required IReadOnlyList<ItemExtraidoValueObject> ItensExtraidos { get; init; }
    public int TokensConsumidos { get; init; }
    public required string ModeloUtilizado { get; init; }
}
