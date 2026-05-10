using Microsoft.Extensions.Logging;
using Polly;
using Polly.Extensions.Http;

namespace Constriva.Messaging.Policies;

public static class RetryPolicy
{
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
