using System.Text.Json.Serialization;

namespace Constriva.Messaging.Models.Lens.ExtractedData.Shared;

/// <summary>
/// Item de medição extraído de um boletim de medição.
/// </summary>
public class ItemMedicaoExtraido
{
    [JsonPropertyName("codigo")]
    public string? Codigo { get; set; }

    [JsonPropertyName("descricao")]
    public string? Descricao { get; set; }

    [JsonPropertyName("unidade")]
    public string? Unidade { get; set; }

    [JsonPropertyName("quantidade_contratada")]
    public decimal? QuantidadeContratada { get; set; }

    [JsonPropertyName("quantidade_anterior")]
    public decimal? QuantidadeAnterior { get; set; }

    [JsonPropertyName("quantidade_atual")]
    public decimal? QuantidadeAtual { get; set; }

    [JsonPropertyName("quantidade_total")]
    public decimal? QuantidadeTotal { get; set; }

    [JsonPropertyName("preco_unitario")]
    public decimal? PrecoUnitario { get; set; }

    [JsonPropertyName("valor_atual")]
    public decimal? ValorAtual { get; set; }

    [JsonPropertyName("valor_acumulado")]
    public decimal? ValorAcumulado { get; set; }

    [JsonPropertyName("percentual_conclusao")]
    public decimal? PercentualConclusao { get; set; }
}
