using System.Text.Json.Serialization;

namespace Constriva.Infrastructure.Integrations.WhatsApp.Dtos.Outbound;

public class EnviarTemplateRequest
{
    [JsonPropertyName("messaging_product")] public string MessagingProduct { get; set; } = "whatsapp";
    [JsonPropertyName("to")] public string To { get; set; } = null!;
    [JsonPropertyName("type")] public string Type { get; set; } = "template";
    [JsonPropertyName("template")] public TemplateBody Template { get; set; } = null!;
}

public class TemplateBody
{
    [JsonPropertyName("name")] public string Name { get; set; } = null!;
    [JsonPropertyName("language")] public TemplateLanguage Language { get; set; } = null!;
    [JsonPropertyName("components")] public List<TemplateComponent> Components { get; set; } = [];
}

public class TemplateLanguage
{
    [JsonPropertyName("code")] public string Code { get; set; } = null!;
}

public class TemplateComponent
{
    [JsonPropertyName("type")] public string Type { get; set; } = null!;
    [JsonPropertyName("parameters")] public List<TemplateParameter> Parameters { get; set; } = [];
    [JsonPropertyName("sub_type")] [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] public string? SubType { get; set; }
    [JsonPropertyName("index")] [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] public int? Index { get; set; }
}

public class TemplateParameter
{
    [JsonPropertyName("type")] public string Type { get; set; } = "text";
    [JsonPropertyName("text")] [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] public string? Text { get; set; }
}
