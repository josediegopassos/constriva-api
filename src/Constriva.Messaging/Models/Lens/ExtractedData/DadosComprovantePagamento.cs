using System.Text.Json.Serialization;
using Constriva.Messaging.Models.Lens.ExtractedData.Shared;

namespace Constriva.Messaging.Models.Lens.ExtractedData;

/// <summary>
/// Dados extraídos de um Comprovante de Pagamento.
/// </summary>
public class DadosComprovantePagamento
{
    [JsonPropertyName("tipo_pagamento")]
    public string? TipoPagamento { get; set; }

    [JsonPropertyName("banco")]
    public string? Banco { get; set; }

    [JsonPropertyName("data_pagamento")]
    public string? DataPagamento { get; set; }

    [JsonPropertyName("hora_pagamento")]
    public string? HoraPagamento { get; set; }

    [JsonPropertyName("pagador")]
    public ParteExtraida? Pagador { get; set; }

    [JsonPropertyName("beneficiario")]
    public ParteExtraida? Beneficiario { get; set; }

    [JsonPropertyName("valor")]
    public decimal? Valor { get; set; }

    [JsonPropertyName("numero_autenticacao")]
    public string? NumeroAutenticacao { get; set; }

    [JsonPropertyName("codigo_transacao")]
    public string? CodigoTransacao { get; set; }

    [JsonPropertyName("descricao")]
    public string? Descricao { get; set; }
}
