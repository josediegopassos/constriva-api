using System.Text.Json.Serialization;
using Constriva.Messaging.Models.Lens.ExtractedData.Shared;

namespace Constriva.Messaging.Models.Lens.ExtractedData;

/// <summary>
/// Dados extraídos de uma Nota Fiscal de Consumidor Eletrônica (NFC-e).
/// </summary>
public class DadosNfce
{
    [JsonPropertyName("numero")]
    public string? Numero { get; set; }

    [JsonPropertyName("serie")]
    public string? Serie { get; set; }

    [JsonPropertyName("chave_acesso")]
    public string? ChaveAcesso { get; set; }

    [JsonPropertyName("data_emissao")]
    public string? DataEmissao { get; set; }

    [JsonPropertyName("emitente")]
    public ParteExtraida? Emitente { get; set; }

    [JsonPropertyName("consumidor")]
    public ParteExtraida? Consumidor { get; set; }

    [JsonPropertyName("itens")]
    public List<ItemExtraido> Itens { get; set; } = new();

    [JsonPropertyName("subtotal")]
    public decimal? Subtotal { get; set; }

    [JsonPropertyName("valor_desconto")]
    public decimal? ValorDesconto { get; set; }

    [JsonPropertyName("valor_total")]
    public decimal? ValorTotal { get; set; }

    [JsonPropertyName("forma_pagamento")]
    public string? FormaPagamento { get; set; }

    [JsonPropertyName("valor_pago")]
    public decimal? ValorPago { get; set; }

    [JsonPropertyName("troco")]
    public decimal? Troco { get; set; }

    [JsonPropertyName("informacoes_complementares")]
    public string? InformacoesComplementares { get; set; }
}
