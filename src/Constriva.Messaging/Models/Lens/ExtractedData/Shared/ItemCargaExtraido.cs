using System.Text.Json.Serialization;

namespace Constriva.Messaging.Models.Lens.ExtractedData.Shared;

/// <summary>
/// Item de carga extraído de um CT-e.
/// </summary>
public class ItemCargaExtraido
{
    [JsonPropertyName("descricao")]
    public string? Descricao { get; set; }

    [JsonPropertyName("quantidade")]
    public decimal? Quantidade { get; set; }

    [JsonPropertyName("unidade")]
    public string? Unidade { get; set; }

    [JsonPropertyName("preco_unitario")]
    public decimal? PrecoUnitario { get; set; }

    [JsonPropertyName("valor_total")]
    public decimal? ValorTotal { get; set; }

    [JsonPropertyName("peso_kg")]
    public decimal? PesoKg { get; set; }
}
