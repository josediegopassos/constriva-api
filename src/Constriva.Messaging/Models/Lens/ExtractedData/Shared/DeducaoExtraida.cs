using System.Text.Json.Serialization;

namespace Constriva.Messaging.Models.Lens.ExtractedData.Shared;

/// <summary>
/// Dedução ou retenção extraída de um documento fiscal de serviço.
/// </summary>
public class DeducaoExtraida
{
    [JsonPropertyName("descricao")]
    public string? Descricao { get; set; }

    [JsonPropertyName("aliquota")]
    public decimal? Aliquota { get; set; }

    [JsonPropertyName("valor")]
    public decimal? Valor { get; set; }
}
