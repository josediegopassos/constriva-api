using System.Text.Json.Serialization;

namespace Constriva.Messaging.Models.Lens.ExtractedData.Shared;

/// <summary>
/// Item de produto/serviço extraído de um documento fiscal.
/// </summary>
public class ItemExtraido
{
    [JsonPropertyName("codigo")]
    public string? Codigo { get; set; }

    [JsonPropertyName("descricao")]
    public string? Descricao { get; set; }

    [JsonPropertyName("ncm")]
    public string? Ncm { get; set; }

    [JsonPropertyName("cfop")]
    public string? Cfop { get; set; }

    [JsonPropertyName("unidade")]
    public string? Unidade { get; set; }

    [JsonPropertyName("quantidade")]
    public decimal? Quantidade { get; set; }

    [JsonPropertyName("preco_unitario")]
    public decimal? PrecoUnitario { get; set; }

    [JsonPropertyName("preco_total")]
    public decimal? PrecoTotal { get; set; }

    [JsonPropertyName("desconto")]
    public decimal? Desconto { get; set; }

    [JsonPropertyName("adicional")]
    public decimal? Adicional { get; set; }

    [JsonPropertyName("aliquota_icms")]
    public decimal? AliquotaIcms { get; set; }

    [JsonPropertyName("aliquota_ipi")]
    public decimal? AliquotaIpi { get; set; }
}
