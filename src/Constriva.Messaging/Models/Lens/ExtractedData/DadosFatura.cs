using System.Text.Json.Serialization;
using Constriva.Messaging.Models.Lens.ExtractedData.Shared;

namespace Constriva.Messaging.Models.Lens.ExtractedData;

/// <summary>
/// Dados extraídos de uma Fatura.
/// </summary>
public class DadosFatura
{
    [JsonPropertyName("numero")]
    public string? Numero { get; set; }

    [JsonPropertyName("data_emissao")]
    public string? DataEmissao { get; set; }

    [JsonPropertyName("data_vencimento")]
    public string? DataVencimento { get; set; }

    [JsonPropertyName("emitente")]
    public ParteExtraida? Emitente { get; set; }

    [JsonPropertyName("destinatario")]
    public ParteExtraida? Destinatario { get; set; }

    [JsonPropertyName("itens")]
    public List<ItemFaturaExtraido> Itens { get; set; } = new();

    [JsonPropertyName("subtotal")]
    public decimal? Subtotal { get; set; }

    [JsonPropertyName("total_impostos")]
    public decimal? TotalImpostos { get; set; }

    [JsonPropertyName("valor_total")]
    public decimal? ValorTotal { get; set; }

    [JsonPropertyName("condicoes_pagamento")]
    public string? CondicoesPagamento { get; set; }

    [JsonPropertyName("observacoes")]
    public string? Observacoes { get; set; }
}
