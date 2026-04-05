using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Logging;

namespace Constriva.Messaging.Services.Notification;

/// <summary>
/// Implementação do serviço de notificações SignalR via endpoint interno da Constriva.API.
/// </summary>
public class SignalRNotificationService : ISignalRNotificationService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger<SignalRNotificationService> _logger;

    /// <summary>
    /// Inicializa uma nova instância do serviço de notificação SignalR.
    /// </summary>
    public SignalRNotificationService(
        IHttpClientFactory httpClientFactory,
        ILogger<SignalRNotificationService> logger)
    {
        _httpClientFactory = httpClientFactory;
        _logger = logger;
    }

    /// <inheritdoc />
    public Task NotificarProcessamentoAtualizadoAsync(Guid usuarioId, Guid? obraId, Guid empresaId, object dados, CancellationToken ct)
        => EnviarNotificacaoAsync("LensProcessamentoAtualizado", usuarioId, obraId, empresaId, dados, ct);

    /// <inheritdoc />
    public Task NotificarProcessamentoConcluidoAsync(Guid usuarioId, Guid? obraId, Guid empresaId, object dados, CancellationToken ct)
        => EnviarNotificacaoAsync("LensProcessamentoConcluido", usuarioId, obraId, empresaId, dados, ct);

    /// <inheritdoc />
    public Task NotificarProcessamentoErroAsync(Guid usuarioId, Guid? obraId, Guid empresaId, object dados, CancellationToken ct)
        => EnviarNotificacaoAsync("LensProcessamentoErro", usuarioId, obraId, empresaId, dados, ct);

    /// <inheritdoc />
    public Task NotificarItemAtualizadoAsync(Guid usuarioId, Guid? obraId, Guid empresaId, object dados, CancellationToken ct)
        => EnviarNotificacaoAsync("LensItemAtualizado", usuarioId, obraId, empresaId, dados, ct);

    /// <inheritdoc />
    public Task NotificarConsolidacaoConcluidaAsync(Guid usuarioId, Guid? obraId, Guid empresaId, object dados, CancellationToken ct)
        => EnviarNotificacaoAsync("LensConsolidacaoConcluida", usuarioId, obraId, empresaId, dados, ct);

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
