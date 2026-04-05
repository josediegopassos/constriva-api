using System.Text.Json.Serialization;
using Constriva.Messaging.Models.Lens.ExtractedData.Shared;

namespace Constriva.Messaging.Models.Lens.ExtractedData;

/// <summary>
/// Dados extraídos de um Recibo.
/// </summary>
public class DadosRecibo
{
    [JsonPropertyName("numero")]
    public string? Numero { get; set; }

    [JsonPropertyName("data_emissao")]
    public string? DataEmissao { get; set; }

    [JsonPropertyName("pagador")]
    public ParteExtraida? Pagador { get; set; }

    [JsonPropertyName("recebedor")]
    public ParteExtraida? Recebedor { get; set; }

    [JsonPropertyName("valor")]
    public decimal? Valor { get; set; }

    [JsonPropertyName("valor_extenso")]
    public string? ValorExtenso { get; set; }

    [JsonPropertyName("referente")]
    public string? Referente { get; set; }

    [JsonPropertyName("forma_pagamento")]
    public string? FormaPagamento { get; set; }
}
