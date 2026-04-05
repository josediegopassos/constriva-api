using System.Text.Json.Serialization;

namespace Constriva.Messaging.Models.Lens.ExtractedData.Shared;

/// <summary>
/// Item detalhado de uma fatura.
/// </summary>
public class ItemFaturaExtraido
{
    [JsonPropertyName("descricao")]
    public string? Descricao { get; set; }

    [JsonPropertyName("referencia")]
    public string? Referencia { get; set; }

    [JsonPropertyName("unidade")]
    public string? Unidade { get; set; }

    [JsonPropertyName("quantidade")]
    public decimal? Quantidade { get; set; }

    [JsonPropertyName("preco_unitario")]
    public decimal? PrecoUnitario { get; set; }

    [JsonPropertyName("preco_total")]
    public decimal? PrecoTotal { get; set; }

    [JsonPropertyName("aliquota_imposto")]
    public decimal? AliquotaImposto { get; set; }

    [JsonPropertyName("valor_imposto")]
    public decimal? ValorImposto { get; set; }
}
