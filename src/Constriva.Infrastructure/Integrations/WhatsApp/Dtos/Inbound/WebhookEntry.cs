using System.Text.Json.Serialization;

namespace Constriva.Infrastructure.Integrations.WhatsApp.Dtos.Inbound;

public class WebhookEntry
{
    [JsonPropertyName("id")] public string? Id { get; set; }
    [JsonPropertyName("changes")] public List<WebhookChange>? Changes { get; set; }
}
