using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Logging;

namespace Constriva.Messaging.Services.Notification;

public class SignalRNotificationService : ISignalRNotificationService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger<SignalRNotificationService> _logger;

    public SignalRNotificationService(
        IHttpClientFactory httpClientFactory,
        ILogger<SignalRNotificationService> logger)
    {
        _httpClientFactory = httpClientFactory;
        _logger = logger;
    }

    public Task NotificarProcessamentoAtualizadoAsync(Guid usuarioId, Guid? obraId, Guid empresaId, object dados, CancellationToken ct)
        => EnviarNotificacaoAsync("LensProcessamentoAtualizado", usuarioId, obraId, empresaId, dados, ct);

    public Task NotificarProcessamentoConcluidoAsync(Guid usuarioId, Guid? obraId, Guid empresaId, object dados, CancellationToken ct)
        => EnviarNotificacaoAsync("LensProcessamentoConcluido", usuarioId, obraId, empresaId, dados, ct);

    public Task NotificarProcessamentoErroAsync(Guid usuarioId, Guid? obraId, Guid empresaId, object dados, CancellationToken ct)
        => EnviarNotificacaoAsync("LensProcessamentoErro", usuarioId, obraId, empresaId, dados, ct);

    public Task NotificarItemAtualizadoAsync(Guid usuarioId, Guid? obraId, Guid empresaId, object dados, CancellationToken ct)
        => EnviarNotificacaoAsync("LensItemAtualizado", usuarioId, obraId, empresaId, dados, ct);

    public Task NotificarConsolidacaoConcluidaAsync(Guid usuarioId, Guid? obraId, Guid empresaId, object dados, CancellationToken ct)
        => EnviarNotificacaoAsync("LensConsolidacaoConcluida", usuarioId, obraId, empresaId, dados, ct);

    public async Task NotificarWhatsAppAtualizacaoAsync(Guid cotacaoId, Guid empresaId, object dados, CancellationToken ct)
    {
        try
        {
            var client = _httpClientFactory.CreateClient("ConstrivaApi");
            var payload = new { cotacaoId, empresaId, dados };
            var json = JsonSerializer.Serialize(payload);
            var conteudo = new StringContent(json, Encoding.UTF8, "application/json");

            var resposta = await client.PostAsync("/api/interno/whatsapp/notificar", conteudo, ct);

            if (!resposta.IsSuccessStatusCode)
            {
                var corpoErro = await resposta.Content.ReadAsStringAsync(ct);
                _logger.LogWarning("Falha ao enviar notificação WhatsApp SignalR. Status: {Status}, Corpo: {Corpo}",
                    resposta.StatusCode, corpoErro);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao enviar notificação WhatsApp SignalR para cotação {CotacaoId}.", cotacaoId);
        }
    }

    private async Task EnviarNotificacaoAsync(string evento, Guid usuarioId, Guid? obraId, Guid empresaId, object dados, CancellationToken ct)
    {
        try
        {
            var client = _httpClientFactory.CreateClient("ConstrivaApi");

            var payload = new
            {
                evento,
                usuarioId,
                obraId,
                empresaId,
                dados
            };

            var json = JsonSerializer.Serialize(payload);
            var conteudo = new StringContent(json, Encoding.UTF8, "application/json");

            var resposta = await client.PostAsync($"/api/interno/lens/notificar/{evento}", conteudo, ct);

            if (!resposta.IsSuccessStatusCode)
            {
                var corpoErro = await resposta.Content.ReadAsStringAsync(ct);
                _logger.LogWarning(
                    "Falha ao enviar notificação SignalR '{Evento}'. Status: {Status}, Corpo: {Corpo}",
                    evento, resposta.StatusCode, corpoErro);
            }
            else
            {
                _logger.LogDebug("Notificação SignalR '{Evento}' enviada com sucesso para usuário {UsuarioId}.", evento, usuarioId);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao enviar notificação SignalR '{Evento}' para usuário {UsuarioId}.", evento, usuarioId);
        }
    }
}
