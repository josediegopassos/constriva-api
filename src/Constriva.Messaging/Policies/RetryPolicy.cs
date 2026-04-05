using Microsoft.Extensions.Logging;
using Polly;
using Polly.Extensions.Http;

namespace Constriva.Messaging.Policies;

/// <summary>
/// Política de retentativa com backoff exponencial para chamadas HTTP ao Constriva.Lens.
/// </summary>
public static class RetryPolicy
{
    /// <summary>
    /// Cria uma política de retentativa assíncrona com 3 tentativas e backoff exponencial.
    /// </summary>
    public static IAsyncPolicy<HttpResponseMessage> Criar(IServiceProvider serviceProvider)
    {
        var logger = serviceProvider.GetRequiredService<ILogger<HttpClient>>();

        return HttpPolicyExtensions
            .HandleTransientHttpError()
            .OrResult(r => (int)r.StatusCode >= 500)
            .WaitAndRetryAsync(
                retryCount: 3,
                sleepDurationProvider: tentativa => TimeSpan.FromSeconds(Math.Pow(2, tentativa) * 2.5),
                onRetry: (resultado, tempo, tentativa, contexto) =>
                {
                    var mensagemErro = resultado.Exception?.Message ?? $"Status HTTP {resultado.Result?.StatusCode}";
                    logger.LogWarning(
                        "Tentativa {Tentativa}/3 para Constriva.Lens falhou: {Erro}. Próxima tentativa em {Tempo}s.",
                        tentativa, mensagemErro, tempo.TotalSeconds);
                });
    }
}
