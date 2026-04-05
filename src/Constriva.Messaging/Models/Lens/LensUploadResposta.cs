using System.Text.Json.Serialization;

namespace Constriva.Messaging.Models.Lens;

/// <summary>
/// Modelo de resposta do upload de arquivo para o Constriva.Lens.
/// </summary>
public class LensUploadResposta
{
    /// <summary>Indica se o upload foi aceito.</summary>
    [JsonPropertyName("accepted")]
    public bool Aceito { get; set; }

    /// <summary>Mensagem de retorno.</summary>
    [JsonPropertyName("message")]
    public string Mensagem { get; set; } = string.Empty;

    /// <summary>Identificador do processamento no Lens.</summary>
    [JsonPropertyName("processing_id")]
    public string? ProcessingId { get; set; }
}
