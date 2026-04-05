using System.Text.Json.Serialization;

namespace Constriva.Messaging.Models.Lens.ExtractedData.Shared;

/// <summary>
/// Item detalhado de um boleto bancário.
/// </summary>
public class ItemBoletoExtraido
{
    [JsonPropertyName("descricao")]
    public string? Descricao { get; set; }

    [JsonPropertyName("referencia")]
    public string? Referencia { get; set; }

    [JsonPropertyName("quantidade")]
    public decimal? Quantidade { get; set; }

    [JsonPropertyName("preco_unitario")]
    public decimal? PrecoUnitario { get; set; }

    [JsonPropertyName("preco_total")]
    public decimal? PrecoTotal { get; set; }
}
