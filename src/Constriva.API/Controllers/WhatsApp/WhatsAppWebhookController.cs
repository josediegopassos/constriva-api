using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Constriva.API.Filters;
using Constriva.Application.Features.Compras.WhatsApp.Commands;
using Constriva.Domain.Enums;
using Constriva.Domain.Interfaces.WhatsApp;
using Constriva.Infrastructure.Integrations.WhatsApp.Dtos.Inbound;
using Constriva.Infrastructure.Integrations.WhatsApp.Options;
using Constriva.Infrastructure.Persistence;
using MediatR;

namespace Constriva.API.Controllers.WhatsApp;

[Route("api/whatsapp/webhook")]
[ApiController]
[AllowAnonymous]
public class WhatsAppWebhookController : ControllerBase
{
    private readonly IWhatsAppGateway _gateway;
    private readonly IMediator _mediator;
    private readonly AppDbContext _db;
    private readonly WhatsAppOptions _options;
    private readonly ILogger<WhatsAppWebhookController> _logger;

    public WhatsAppWebhookController(
        IWhatsAppGateway gateway,
        IMediator mediator,
        AppDbContext db,
        IOptions<WhatsAppOptions> options,
        ILogger<WhatsAppWebhookController> logger)
    {
        _gateway = gateway;
        _mediator = mediator;
        _db = db;
        _options = options.Value;
        _logger = logger;
    }

    [HttpGet]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public IActionResult VerificarWebhook(
        [FromQuery(Name = "hub.mode")] string mode,
        [FromQuery(Name = "hub.verify_token")] string token,
        [FromQuery(Name = "hub.challenge")] string challenge)
    {
        _logger.LogInformation("Verificação de webhook recebida. Mode: {Mode}", mode);

        if (!_gateway.ValidarTokenVerificacao(mode, token, challenge, out var challengeResponse))
        {
            _logger.LogWarning("Verificação de webhook falhou — token inválido");
            return Forbid();
        }

        _logger.LogInformation("Webhook verificado com sucesso");
        return Content(challengeResponse, "text/plain");
    }

    [HttpPost]
    [RawBody]
    [Consumes("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> ReceberEvento(
        [FromBody] WebhookPayload payload,
        CancellationToken cancellationToken)
    {
        var rawBody = HttpContext.Items["RawBody"] as string;
        var assinatura = Request.Headers["X-Hub-Signature-256"].FirstOrDefault();

        if (string.IsNullOrEmpty(rawBody) || string.IsNullOrEmpty(assinatura))
            return Unauthorized("Assinatura ausente");

        if (!_gateway.ValidarAssinaturaWebhook(rawBody, assinatura))
        {
            _logger.LogWarning("Assinatura HMAC-SHA256 inválida. IP: {IP}",
                HttpContext.Connection.RemoteIpAddress);
            return Unauthorized("Assinatura inválida");
        }

        if (payload?.Entry == null || !payload.Entry.Any())
            return Ok();

        foreach (var entry in payload.Entry)
        {
            foreach (var change in entry.Changes ?? [])
            {
                if (change.Field != "messages" || change.Value == null)
                    continue;

                foreach (var msg in change.Value.Messages ?? [])
                    await ProcessarMensagemAsync(msg, change.Value, cancellationToken);

                foreach (var status in change.Value.Statuses ?? [])
                    ProcessarStatusUpdate(status);
            }
        }

        return Ok();
    }

    private async Task ProcessarMensagemAsync(
        WebhookMessage mensagem, WebhookValue value, CancellationToken ct)
    {
        if (string.IsNullOrEmpty(mensagem.Id) || string.IsNullOrEmpty(mensagem.From))
            return;

        var recebidaEm = DateTime.UtcNow;
        if (!string.IsNullOrEmpty(mensagem.Timestamp) &&
            long.TryParse(mensagem.Timestamp, out var unix))
            recebidaEm = DateTimeOffset.FromUnixTimeSeconds(unix).UtcDateTime;

        var tipoConteudo = mensagem.Type?.ToLower() switch
        {
            "text" => TipoConteudoMensagemEnum.Texto,
            "image" => TipoConteudoMensagemEnum.Imagem,
            "document" => TipoConteudoMensagemEnum.Documento,
            "audio" => TipoConteudoMensagemEnum.Audio,
            _ => TipoConteudoMensagemEnum.Desconhecido
        };

        _logger.LogInformation(
            "Mensagem recebida. WaMessageId: {Id} | Tipo: {Tipo} | De: {De}",
            mensagem.Id, tipoConteudo, Redact(mensagem.From));

        try
        {
            var telefoneNormalizado = Normalizar(mensagem.From);

            _logger.LogInformation(
                "Buscando mensagem para telefone normalizado: {Tel} (original: {Original})",
                telefoneNormalizado, mensagem.From);

            var totalMensagens = await _db.MensagensWhatsApp.CountAsync(ct);
            var telefones = await _db.MensagensWhatsApp
                .Where(m => !m.IsDeleted)
                .Select(m => m.TelefoneDestino)
                .Distinct()
                .ToListAsync(ct);

            _logger.LogInformation(
                "Total mensagens no banco: {Total} | Telefones cadastrados: [{Telefones}]",
                totalMensagens, string.Join(", ", telefones));

            var empresaId = await _db.MensagensWhatsApp
                .Where(m => m.TelefoneDestino == telefoneNormalizado && !m.IsDeleted)
                .OrderByDescending(m => m.CreatedAt)
                .Select(m => m.EmpresaId)
                .FirstOrDefaultAsync(ct);

            if (empresaId == Guid.Empty)
            {
                _logger.LogWarning("Telefone {Tel} não encontrado em nenhuma mensagem enviada", Redact(mensagem.From));
                return;
            }

            await _mediator.Send(new RegistrarRespostaFornecedorCommand
            {
                EmpresaId = empresaId,
                WaMessageId = mensagem.Id,
                TelefoneOrigem = Normalizar(mensagem.From),
                TelefoneDestino = Normalizar(value.Metadata?.PhoneNumberId ?? _options.PhoneNumberId),
                RecebidaEm = recebidaEm,
                TipoConteudo = tipoConteudo,
                TextoMensagem = mensagem.Text?.Body,
                WaMediaId = mensagem.Image?.Id ?? mensagem.Document?.Id ?? mensagem.Audio?.Id,
                MediaMimeType = mensagem.Image?.MimeType ?? mensagem.Document?.MimeType ?? mensagem.Audio?.MimeType,
                MediaNomeArquivo = mensagem.Document?.Filename,
                PayloadWebhookOriginal = JsonSerializer.Serialize(mensagem)
            }, ct);
        }
        catch (KeyNotFoundException ex)
        {
            _logger.LogInformation("Mensagem de {Tel} sem cotação ativa: {Msg}",
                Redact(mensagem.From), ex.Message);
        }
        catch (Exception ex) when (ex is not OperationCanceledException)
        {
            _logger.LogError(ex, "Erro ao processar mensagem {Id}", mensagem.Id);
        }
    }

    private void ProcessarStatusUpdate(WebhookStatus status)
    {
        if (string.IsNullOrEmpty(status.Id))
            return;

        if (status.Status == "failed")
        {
            var erros = status.Errors?.Select(e => $"[{e.Code}] {e.Title}: {e.Message}") ?? [];
            _logger.LogWarning("Falha na entrega {Id}. Para: {Para} | Erros: {Erros}",
                status.Id, Redact(status.RecipientId ?? ""), string.Join("; ", erros));
        }
        else
        {
            _logger.LogInformation("Status update: {Id} → {Status}", status.Id, status.Status);
        }
    }

    private static string Normalizar(string tel)
    {
        if (string.IsNullOrEmpty(tel))
            return tel;

        var digitos = new string(tel.Where(char.IsDigit).ToArray());

        if (!digitos.StartsWith("55"))
            digitos = "55" + digitos;

        // Celulares brasileiros: 55 + DDD(2) + 9 + número(8) = 13 dígitos
        // A Meta às vezes envia sem o nono dígito (12 dígitos)
        if (digitos.Length == 12)
        {
            var ddd = digitos[2..4];
            var numero = digitos[4..];
            digitos = $"55{ddd}9{numero}";
        }

        return $"+{digitos}";
    }

    private static string Redact(string tel)
        => tel.Length > 4 ? $"***{tel[^4..]}" : "****";
}
