using System.Text.Json.Serialization;

namespace Constriva.Infrastructure.Integrations.WhatsApp.Dtos.Inbound;

public class WebhookTextContent
{
    [JsonPropertyName("body")] public string? Body { get; set; }
}
