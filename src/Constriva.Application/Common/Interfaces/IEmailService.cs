namespace Constriva.Application.Common.Interfaces;

public interface IEmailService
{
    Task SendAsync(string to, string subject, string body, CancellationToken ct = default);
    Task SendTemplateAsync(string to, string template, object model, CancellationToken ct = default);
}
