using System.Text.Json.Serialization;

namespace Constriva.Infrastructure.Integrations.WhatsApp.Dtos.Inbound;

public class WebhookMessage
{
    [JsonPropertyName("from")] public string? From { get; set; }
    [JsonPropertyName("id")] public string? Id { get; set; }
    [JsonPropertyName("timestamp")] public string? Timestamp { get; set; }
    [JsonPropertyName("type")] public string? Type { get; set; }
    [JsonPropertyName("text")] public WebhookTextContent? Text { get; set; }
    [JsonPropertyName("image")] public WebhookImageContent? Image { get; set; }
    [JsonPropertyName("document")] public WebhookDocumentContent? Document { get; set; }
    [JsonPropertyName("audio")] public WebhookAudioContent? Audio { get; set; }
    [JsonPropertyName("context")] public WebhookMessageContext? Context { get; set; }
}

public class WebhookMessageContext
{
    [JsonPropertyName("from")] public string? From { get; set; }
    [JsonPropertyName("id")] public string? Id { get; set; }
}
