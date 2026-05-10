using System.Text.Json.Serialization;

namespace Constriva.Infrastructure.Integrations.WhatsApp.Dtos.Outbound;

public class MarcarComoLidaRequest
{
    [JsonPropertyName("messaging_product")] public string MessagingProduct { get; set; } = "whatsapp";
    [JsonPropertyName("status")] public string Status { get; set; } = "read";
    [JsonPropertyName("message_id")] public string MessageId { get; set; } = null!;
}
