using System.Text.Json.Serialization;

namespace Constriva.Infrastructure.Integrations.WhatsApp.Dtos.Inbound;

public class WebhookValue
{
    [JsonPropertyName("messaging_product")] public string? MessagingProduct { get; set; }
    [JsonPropertyName("metadata")] public WebhookMetadata? Metadata { get; set; }
    [JsonPropertyName("contacts")] public List<WebhookContact>? Contacts { get; set; }
    [JsonPropertyName("messages")] public List<WebhookMessage>? Messages { get; set; }
    [JsonPropertyName("statuses")] public List<WebhookStatus>? Statuses { get; set; }
}

public class WebhookMetadata
{
    [JsonPropertyName("display_phone_number")] public string? DisplayPhoneNumber { get; set; }
    [JsonPropertyName("phone_number_id")] public string? PhoneNumberId { get; set; }
}
