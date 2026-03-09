using Constriva.Application.Common.Interfaces;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Constriva.Infrastructure.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _config;
        public EmailService(IConfiguration config) => _config = config;

        public async Task SendAsync(string to, string subject, string body, CancellationToken ct = default)
        {
            var message = new MimeMessage();
            message.From.Add(MailboxAddress.Parse(_config["Email:From"]));
            message.To.Add(MailboxAddress.Parse(to));
            message.Subject = subject;
            message.Body = new TextPart("html") { Text = body };

            using var client = new SmtpClient();
            await client.ConnectAsync(_config["Email:Host"], int.Parse(_config["Email:Port"] ?? "587"), false, ct);
            await client.AuthenticateAsync(_config["Email:User"], _config["Email:Password"], ct);
            await client.SendAsync(message, ct);
            await client.DisconnectAsync(true, ct);
        }

        public async Task SendTemplateAsync(string to, string template, object model, CancellationToken ct = default)
        {
            // In production, use Razor templates or SendGrid
            var body = $"<h1>Sistema Obras</h1><p>Template: {template}</p><pre>{JsonSerializer.Serialize(model)}</pre>";
            await SendAsync(to, $"Sistema Obras - {template}", body, ct);
        }
    }
}