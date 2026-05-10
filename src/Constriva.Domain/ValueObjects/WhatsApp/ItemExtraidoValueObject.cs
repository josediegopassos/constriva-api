namespace Constriva.Domain.ValueObjects.WhatsApp;

public class ItemExtraidoValueObject
{
    public Guid? ItemCotacaoId { get; init; }
    public required string DescricaoOriginal { get; init; }
    public required decimal PrecoUnitario { get; init; }
    public required decimal Quantidade { get; init; }
    public string? UnidadeMedida { get; init; }
    public string? Marca { get; init; }
    public bool? Disponivel { get; init; }
    public int ConfiancaItem { get; init; }
    public string? Observacao { get; init; }
}
