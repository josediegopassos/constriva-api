using System.ComponentModel.DataAnnotations;

namespace Constriva.Infrastructure.Integrations.WhatsApp.Options;

public class WhatsAppOptions
{
    [Required]
    public string PhoneNumberId { get; set; } = string.Empty;

    [Required]
    public string AccessToken { get; set; } = string.Empty;

    [Required]
    public string VerifyToken { get; set; } = string.Empty;

    [Required]
    public string AppSecret { get; set; } = string.Empty;

    public string ApiVersion { get; set; } = "v25.0";

    public string BaseUrl { get; set; } = "https://graph.facebook.com";

    [Range(5, 120)]
    public int TimeoutSegundos { get; set; } = 30;

    [Range(1, 5)]
    public int MaxTentativas { get; set; } = 3;

    [Required]
    public string TemplateConviteCotacao { get; set; } = string.Empty;

    [Required]
    public string TemplateLembreteCotacao { get; set; } = string.Empty;

    [Required]
    public string TemplateConfirmacaoAprovacao { get; set; } = string.Empty;

    [Range(1, 10)]
    public int MaxLembretes { get; set; } = 2;

    public string TemplateIdioma { get; set; } = "pt_BR";

    internal string MessagesEndpoint => $"{BaseUrl}/{ApiVersion}/{PhoneNumberId}/messages";

    internal string MediaEndpoint(string mediaId) => $"{BaseUrl}/{ApiVersion}/{mediaId}";
}
