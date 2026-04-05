using System.Text.Json.Serialization;

namespace Constriva.Messaging.Models.Lens.ExtractedData.Shared;

/// <summary>
/// Endereço extraído de um documento via OCR.
/// </summary>
public class EnderecoExtraido
{
    [JsonPropertyName("logradouro")]
    public string? Logradouro { get; set; }

    [JsonPropertyName("numero")]
    public string? Numero { get; set; }

    [JsonPropertyName("complemento")]
    public string? Complemento { get; set; }

    [JsonPropertyName("bairro")]
    public string? Bairro { get; set; }

    [JsonPropertyName("cidade")]
    public string? Cidade { get; set; }

    [JsonPropertyName("estado")]
    public string? Estado { get; set; }

    [JsonPropertyName("cep")]
    public string? Cep { get; set; }
}
