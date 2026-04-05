using System.Text.Json.Serialization;
using Constriva.Messaging.Models.Lens.ExtractedData.Shared;

namespace Constriva.Messaging.Models.Lens.ExtractedData;

/// <summary>
/// Dados extraídos de um Boleto Bancário.
/// </summary>
public class DadosBoleto
{
    [JsonPropertyName("banco")]
    public string? Banco { get; set; }

    [JsonPropertyName("codigo_banco")]
    public string? CodigoBanco { get; set; }

    [JsonPropertyName("linha_digitavel")]
    public string? LinhaDigitavel { get; set; }

    [JsonPropertyName("codigo_barras")]
    public string? CodigoBarras { get; set; }

    [JsonPropertyName("beneficiario")]
    public ParteExtraida? Beneficiario { get; set; }

    [JsonPropertyName("pagador")]
    public ParteExtraida? Pagador { get; set; }

    [JsonPropertyName("data_vencimento")]
    public string? DataVencimento { get; set; }

    [JsonPropertyName("data_documento")]
    public string? DataDocumento { get; set; }

    [JsonPropertyName("numero_documento")]
    public string? NumeroDocumento { get; set; }

    [JsonPropertyName("nosso_numero")]
    public string? NossoNumero { get; set; }

    [JsonPropertyName("valor_documento")]
    public decimal? ValorDocumento { get; set; }

    [JsonPropertyName("valor_desconto")]
    public decimal? ValorDesconto { get; set; }

    [JsonPropertyName("valor_multa")]
    public decimal? ValorMulta { get; set; }

    [JsonPropertyName("valor_juros")]
    public decimal? ValorJuros { get; set; }

    [JsonPropertyName("valor_cobrado")]
    public decimal? ValorCobrado { get; set; }

    [JsonPropertyName("itens")]
    public List<ItemBoletoExtraido> Itens { get; set; } = new();

    [JsonPropertyName("instrucoes")]
    public string? Instrucoes { get; set; }
}
