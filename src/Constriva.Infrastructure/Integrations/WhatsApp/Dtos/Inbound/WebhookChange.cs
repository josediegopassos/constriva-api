using System.Text.Json.Serialization;

namespace Constriva.Infrastructure.Integrations.WhatsApp.Dtos.Inbound;

public class WebhookChange
{
    [JsonPropertyName("value")] public WebhookValue? Value { get; set; }
    [JsonPropertyName("field")] public string? Field { get; set; }
}
