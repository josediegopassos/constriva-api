using System.Text.Json.Serialization;
using Constriva.Messaging.Models.Lens.ExtractedData.Shared;

namespace Constriva.Messaging.Models.Lens.ExtractedData;

/// <summary>
/// Dados extraídos de um Conhecimento de Transporte Eletrônico (CT-e).
/// </summary>
public class DadosCte
{
    [JsonPropertyName("numero")]
    public string? Numero { get; set; }

    [JsonPropertyName("serie")]
    public string? Serie { get; set; }

    [JsonPropertyName("chave_acesso")]
    public string? ChaveAcesso { get; set; }

    [JsonPropertyName("data_emissao")]
    public string? DataEmissao { get; set; }

    [JsonPropertyName("cfop")]
    public string? Cfop { get; set; }

    [JsonPropertyName("natureza_operacao")]
    public string? NaturezaOperacao { get; set; }

    [JsonPropertyName("emitente")]
    public ParteExtraida? Emitente { get; set; }

    [JsonPropertyName("remetente")]
    public ParteExtraida? Remetente { get; set; }

    [JsonPropertyName("destinatario")]
    public ParteExtraida? Destinatario { get; set; }

    [JsonPropertyName("itens_carga")]
    public List<ItemCargaExtraido> ItensCarga { get; set; } = new();

    [JsonPropertyName("valor_frete")]
    public decimal? ValorFrete { get; set; }

    [JsonPropertyName("valor_total")]
    public decimal? ValorTotal { get; set; }

    [JsonPropertyName("peso_total_kg")]
    public decimal? PesoTotalKg { get; set; }

    [JsonPropertyName("informacoes_complementares")]
    public string? InformacoesComplementares { get; set; }
}
