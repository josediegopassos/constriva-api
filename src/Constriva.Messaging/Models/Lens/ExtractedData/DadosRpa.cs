using System.Text.Json.Serialization;
using Constriva.Messaging.Models.Lens.ExtractedData.Shared;

namespace Constriva.Messaging.Models.Lens.ExtractedData;

/// <summary>
/// Dados extraídos de um Recibo de Pagamento Autônomo (RPA).
/// </summary>
public class DadosRpa
{
    [JsonPropertyName("numero")]
    public string? Numero { get; set; }

    [JsonPropertyName("data_emissao")]
    public string? DataEmissao { get; set; }

    [JsonPropertyName("contratante")]
    public ParteExtraida? Contratante { get; set; }

    [JsonPropertyName("contratado")]
    public ParteExtraida? Contratado { get; set; }

    [JsonPropertyName("descricao_servico")]
    public string? DescricaoServico { get; set; }

    [JsonPropertyName("valor_bruto")]
    public decimal? ValorBruto { get; set; }

    [JsonPropertyName("inss")]
    public decimal? Inss { get; set; }

    [JsonPropertyName("irrf")]
    public decimal? Irrf { get; set; }

    [JsonPropertyName("iss")]
    public decimal? Iss { get; set; }

    [JsonPropertyName("outras_deducoes")]
    public decimal? OutrasDeducoes { get; set; }

    [JsonPropertyName("valor_liquido")]
    public decimal? ValorLiquido { get; set; }
}
