using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Constriva.Messaging.Services.Lens;

/// <summary>
/// Implementação do serviço de autenticação com o Constriva.Lens.
/// Thread-safe via SemaphoreSlim para evitar múltiplas requisições simultâneas de token.
/// </summary>
public class LensAuthenticationService : ILensAuthenticationService
{
    private const string ChaveCache = "constriva-lens-token";
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IMemoryCache _cache;
    private readonly IConfiguration _configuration;
    private readonly ILogger<LensAuthenticationService> _logger;
    private readonly SemaphoreSlim _semaforo = new(1, 1);

    /// <summary>
    /// Inicializa uma nova instância do serviço de autenticação Lens.
    /// </summary>
    public LensAuthenticationService(
        IHttpClientFactory httpClientFactory,
        IMemoryCache cache,
        IConfiguration configuration,
        ILogger<LensAuthenticationService> logger)
    {
        _httpClientFactory = httpClientFactory;
        _cache = cache;
        _configuration = configuration;
        _logger = logger;
    }

    /// <inheritdoc />
    public async Task<string> ObterTokenAsync(CancellationToken ct)
    {
        if (_cache.TryGetValue(ChaveCache, out string? tokenCache) && !string.IsNullOrEmpty(tokenCache))
            return tokenCache;

        await _semaforo.WaitAsync(ct);
        try
        {
            // Verificar novamente após obter o semáforo (double-check)
            if (_cache.TryGetValue(ChaveCache, out tokenCache) && !string.IsNullOrEmpty(tokenCache))
                return tokenCache;

            _logger.LogInformation("Obtendo novo token de autenticação do Constriva.Lens...");

            var client = _httpClientFactory.CreateClient("ConstrivaLens");
            var clientId = _configuration["ConstrivaLens:ClientId"] ?? "constriva-api";
            var clientSecret = _configuration["ConstrivaLens:ClientSecret"] ?? "";

            var conteudo = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                ["client_id"] = clientId,
                ["client_secret"] = clientSecret,
                ["grant_type"] = "client_credentials"
            });

            var resposta = await client.PostAsync("/auth/token", conteudo, ct);

            if (!resposta.IsSuccessStatusCode)
            {
                var corpoErro = await resposta.Content.ReadAsStringAsync(ct);
                _logger.LogError("Falha ao obter token do Constriva.Lens. Status: {Status}, Corpo: {Corpo}",
                    resposta.StatusCode, corpoErro);
                throw new HttpRequestException($"Falha na autenticação com Constriva.Lens. Status: {resposta.StatusCode}");
            }

            var json = await resposta.Content.ReadAsStringAsync(ct);
            var tokenResposta = JsonSerializer.Deserialize<TokenResposta>(json);

            if (tokenResposta is null || string.IsNullOrEmpty(tokenResposta.AccessToken))
                throw new InvalidOperationException("Token de acesso do Constriva.Lens retornou vazio.");

            // Cache com margem de segurança de 60 segundos
            var expiracao = TimeSpan.FromSeconds(Math.Max(tokenResposta.ExpiresIn - 60, 30));
            _cache.Set(ChaveCache, tokenResposta.AccessToken, expiracao);

            _logger.LogInformation("Token do Constriva.Lens obtido com sucesso. Expira em {Expiracao}s.", expiracao.TotalSeconds);

            return tokenResposta.AccessToken;
        }
        finally
        {
            _semaforo.Release();
        }
    }

    /// <inheritdoc />
    public Task InvalidarTokenAsync()
    {
        _cache.Remove(ChaveCache);
        _logger.LogInformation("Token do Constriva.Lens invalidado.");
        return Task.CompletedTask;
    }

    private class TokenResposta
    {
        [JsonPropertyName("access_token")]
        public string AccessToken { get; set; } = string.Empty;

        [JsonPropertyName("expires_in")]
        public int ExpiresIn { get; set; }

        [JsonPropertyName("token_type")]
        public string TokenType { get; set; } = string.Empty;
    }
}
