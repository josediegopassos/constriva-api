using System.Text.Json.Serialization;

namespace Constriva.Infrastructure.Integrations.WhatsApp.Dtos.Inbound;

public class WebhookError
{
    [JsonPropertyName("code")] public int Code { get; set; }
    [JsonPropertyName("title")] public string? Title { get; set; }
    [JsonPropertyName("message")] public string? Message { get; set; }
    [JsonPropertyName("error_data")] public WebhookErrorData? ErrorData { get; set; }
}

public class WebhookErrorData
{
    [JsonPropertyName("details")] public string? Details { get; set; }
}
