using System.Text.Json.Serialization;

namespace Constriva.Infrastructure.Integrations.WhatsApp.Dtos.Outbound;

public class EnviarMensagemTextRequest
{
    [JsonPropertyName("messaging_product")] public string MessagingProduct { get; set; } = "whatsapp";
    [JsonPropertyName("recipient_type")] public string RecipientType { get; set; } = "individual";
    [JsonPropertyName("to")] public string To { get; set; } = null!;
    [JsonPropertyName("type")] public string Type { get; set; } = "text";
    [JsonPropertyName("text")] public TextBody Text { get; set; } = null!;
    [JsonPropertyName("context")] public MessageContext? Context { get; set; }
}

public class TextBody
{
    [JsonPropertyName("preview_url")] public bool PreviewUrl { get; set; } = false;
    [JsonPropertyName("body")] public string Body { get; set; } = null!;
}

public class MessageContext
{
    [JsonPropertyName("message_id")] public string MessageId { get; set; } = null!;
}
