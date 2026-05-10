using System.Text.Json.Serialization;

namespace Constriva.Infrastructure.Integrations.WhatsApp.Dtos.Outbound;

public class EnviarMensagemResponse
{
    [JsonPropertyName("messaging_product")] public string? MessagingProduct { get; set; }
    [JsonPropertyName("contacts")] public List<ResponseContact>? Contacts { get; set; }
    [JsonPropertyName("messages")] public List<ResponseMessage>? Messages { get; set; }
}

public class ResponseContact
{
    [JsonPropertyName("input")] public string? Input { get; set; }
    [JsonPropertyName("wa_id")] public string? WaId { get; set; }
}

public class ResponseMessage
{
    [JsonPropertyName("id")] public string? Id { get; set; }
    [JsonPropertyName("message_status")] public string? MessageStatus { get; set; }
}
