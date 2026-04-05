using System.Text.Json.Serialization;

namespace Constriva.Messaging.Models.Lens.ExtractedData.Shared;

/// <summary>
/// Parte envolvida em um documento (emitente, destinatário, prestador, tomador).
/// </summary>
public class ParteExtraida
{
    [JsonPropertyName("nome")]
    public string? Nome { get; set; }

    [JsonPropertyName("cnpj")]
    public string? Cnpj { get; set; }

    [JsonPropertyName("cpf")]
    public string? Cpf { get; set; }

    [JsonPropertyName("ie")]
    public string? Ie { get; set; }

    [JsonPropertyName("im")]
    public string? Im { get; set; }

    [JsonPropertyName("endereco")]
    public EnderecoExtraido? Endereco { get; set; }
}
