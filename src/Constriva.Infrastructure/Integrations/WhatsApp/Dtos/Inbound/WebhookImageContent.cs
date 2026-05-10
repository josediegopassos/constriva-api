using System.Text.Json.Serialization;

namespace Constriva.Infrastructure.Integrations.WhatsApp.Dtos.Inbound;

public class WebhookImageContent
{
    [JsonPropertyName("id")] public string? Id { get; set; }
    [JsonPropertyName("mime_type")] public string? MimeType { get; set; }
    [JsonPropertyName("caption")] public string? Caption { get; set; }
    [JsonPropertyName("sha256")] public string? Sha256 { get; set; }
}
