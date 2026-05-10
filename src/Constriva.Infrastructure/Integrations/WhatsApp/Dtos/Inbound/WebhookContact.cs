using System.Text.Json.Serialization;

namespace Constriva.Infrastructure.Integrations.WhatsApp.Dtos.Inbound;

public class WebhookContact
{
    [JsonPropertyName("profile")] public WebhookProfile? Profile { get; set; }
    [JsonPropertyName("wa_id")] public string? WaId { get; set; }
}

public class WebhookProfile
{
    [JsonPropertyName("name")] public string? Name { get; set; }
}
