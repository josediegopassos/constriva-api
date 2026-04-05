using System.Text.Json.Serialization;
using Constriva.Messaging.Models.Lens.ExtractedData.Shared;

namespace Constriva.Messaging.Models.Lens.ExtractedData;

/// <summary>
/// Dados extraídos de uma Nota Fiscal de Serviço Eletrônica (NFS-e).
/// </summary>
public class DadosNfse
{
    [JsonPropertyName("numero")]
    public string? Numero { get; set; }

    [JsonPropertyName("codigo_verificacao")]
    public string? CodigoVerificacao { get; set; }

    [JsonPropertyName("data_emissao")]
    public string? DataEmissao { get; set; }

    [JsonPropertyName("competencia")]
    public string? Competencia { get; set; }

    [JsonPropertyName("prestador")]
    public ParteExtraida? Prestador { get; set; }

    [JsonPropertyName("tomador")]
    public ParteExtraida? Tomador { get; set; }

    [JsonPropertyName("descricao_servico")]
    public string? DescricaoServico { get; set; }

    [JsonPropertyName("codigo_servico")]
    public string? CodigoServico { get; set; }

    [JsonPropertyName("valor_servicos")]
    public decimal? ValorServicos { get; set; }

    [JsonPropertyName("deducoes")]
    public List<DeducaoExtraida> Deducoes { get; set; } = new();

    [JsonPropertyName("base_calculo")]
    public decimal? BaseCalculo { get; set; }

    [JsonPropertyName("aliquota_iss")]
    public decimal? AliquotaIss { get; set; }

    [JsonPropertyName("valor_iss")]
    public decimal? ValorIss { get; set; }

    [JsonPropertyName("iss_retido")]
    public bool? IssRetido { get; set; }

    [JsonPropertyName("valor_liquido")]
    public decimal? ValorLiquido { get; set; }

    [JsonPropertyName("informacoes_complementares")]
    public string? InformacoesComplementares { get; set; }
}
