namespace Constriva.Infrastructure.Integrations.WhatsApp;

public class WhatsAppGatewayException : Exception
{
    public int? HttpStatusCode { get; }

    public string? ResponseBody { get; }

    public WhatsAppGatewayException(string message) : base(message) { }

    public WhatsAppGatewayException(string message, int httpStatusCode, string responseBody)
        : base(message)
    {
        HttpStatusCode = httpStatusCode;
        ResponseBody = responseBody;
    }

    public WhatsAppGatewayException(string message, Exception innerException)
        : base(message, innerException) { }
}
