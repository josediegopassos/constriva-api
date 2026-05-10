namespace Constriva.Domain.ValueObjects.WhatsApp;

public class ItemCotacaoReferenciaValueObject
{
    public required Guid ItemCotacaoId { get; init; }
    public required string Descricao { get; init; }
    public required string UnidadeMedida { get; init; }
    public required decimal Quantidade { get; init; }
    public string? Especificacao { get; init; }
    public decimal? PrecoReferencia { get; init; }
}
