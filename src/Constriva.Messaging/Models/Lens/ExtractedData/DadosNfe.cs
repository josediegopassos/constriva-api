using System.Text.Json.Serialization;
using Constriva.Messaging.Models.Lens.ExtractedData.Shared;

namespace Constriva.Messaging.Models.Lens.ExtractedData;

/// <summary>
/// Dados extraídos de uma Nota Fiscal Eletrônica (NF-e).
/// </summary>
public class DadosNfe
{
    [JsonPropertyName("numero")]
    public string? Numero { get; set; }

    [JsonPropertyName("serie")]
    public string? Serie { get; set; }

    [JsonPropertyName("chave_acesso")]
    public string? ChaveAcesso { get; set; }

    [JsonPropertyName("data_emissao")]
    public string? DataEmissao { get; set; }

    [JsonPropertyName("data_saida")]
    public string? DataSaida { get; set; }

    [JsonPropertyName("natureza_operacao")]
    public string? NaturezaOperacao { get; set; }

    [JsonPropertyName("emitente")]
    public ParteExtraida? Emitente { get; set; }

    [JsonPropertyName("destinatario")]
    public ParteExtraida? Destinatario { get; set; }

    [JsonPropertyName("itens")]
    public List<ItemExtraido> Itens { get; set; } = new();

    [JsonPropertyName("valor_total_produtos")]
    public decimal? ValorTotalProdutos { get; set; }

    [JsonPropertyName("valor_frete")]
    public decimal? ValorFrete { get; set; }

    [JsonPropertyName("valor_seguro")]
    public decimal? ValorSeguro { get; set; }

    [JsonPropertyName("valor_desconto")]
    public decimal? ValorDesconto { get; set; }

    [JsonPropertyName("valor_ipi")]
    public decimal? ValorIpi { get; set; }

    [JsonPropertyName("valor_icms")]
    public decimal? ValorIcms { get; set; }

    [JsonPropertyName("valor_total_nota")]
    public decimal? ValorTotalNota { get; set; }

    [JsonPropertyName("informacoes_complementares")]
    public string? InformacoesComplementares { get; set; }
}
