using System.Text.Json.Serialization;

namespace Constriva.Messaging.Models.Lens;

public class LensUploadResposta
{
    [JsonPropertyName("accepted")]
    public bool Aceito { get; set; }

    [JsonPropertyName("message")]
    public string Mensagem { get; set; } = string.Empty;

    [JsonPropertyName("processing_id")]
    public string? ProcessingId { get; set; }
}
