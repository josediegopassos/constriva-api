using Microsoft.Extensions.Logging;
using Polly;
using Polly.Extensions.Http;

namespace Constriva.Messaging.Policies;

/// <summary>
/// Política de circuit breaker para proteger contra falhas consecutivas do Constriva.Lens.
/// </summary>
public static class CircuitBreakerPolicy
{
    /// <summary>
    /// Cria uma política de circuit breaker que abre após 5 falhas consecutivas por 30 segundos.
    /// </summary>
    public static IAsyncPolicy<HttpResponseMessage> Criar(IServiceProvider serviceProvider)
    {
        var logger = serviceProvider.GetRequiredService<ILogger<HttpClient>>();

        return HttpPolicyExtensions
            .HandleTransientHttpError()
            .OrResult(r => (int)r.StatusCode >= 500)
            .CircuitBreakerAsync(
                handledEventsAllowedBeforeBreaking: 5,
                durationOfBreak: TimeSpan.FromSeconds(30),
                onBreak: (resultado, duracao) =>
                {
                    var mensagemErro = resultado.Exception?.Message ?? $"Status HTTP {resultado.Result?.StatusCode}";
                    logger.LogError(
                        "Circuit breaker ABERTO para Constriva.Lens por {Duracao}s. Motivo: {Erro}",
                        duracao.TotalSeconds, mensagemErro);
                },
                onReset: () =>
                {
                    logger.LogInformation("Circuit breaker FECHADO para Constriva.Lens. Conexão restaurada.");
                },
                onHalfOpen: () =>
                {
                    logger.LogInformation("Circuit breaker SEMI-ABERTO para Constriva.Lens. Testando conexão...");
                });
    }
}
