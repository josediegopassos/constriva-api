using Microsoft.Extensions.Diagnostics.HealthChecks;
using MongoDB.Driver;
using Constriva.Messaging.Configuration;
using Constriva.Messaging.Policies;
using Constriva.Messaging.Repositories.Lens;
using Constriva.Messaging.Services.Lens;
using Constriva.Messaging.Services.Notification;

namespace Constriva.Messaging.Extensions;

/// <summary>
/// Extensões para configuração dos serviços do Constriva.Messaging.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Registra todos os serviços necessários para o Constriva.Messaging.
    /// </summary>
    public static IServiceCollection AdicionarConstrivaMensageria(this IServiceCollection services, IConfiguration configuration)
    {
        // Configurações tipadas
        services.Configure<RabbitMqConfiguration>(configuration.GetSection("RabbitMq"));
        services.Configure<MongoDbConfiguration>(configuration.GetSection("MongoDb"));

        // MassTransit + RabbitMQ
        services.AdicionarMassTransit(configuration);

        // MongoDB
        var mongoConfig = configuration.GetSection("MongoDb").Get<MongoDbConfiguration>() ?? new MongoDbConfiguration();
        services.AddSingleton<IMongoClient>(sp => new MongoClient(mongoConfig.ConnectionString));
        services.AddSingleton(sp =>
        {
            var client = sp.GetRequiredService<IMongoClient>();
            return client.GetDatabase(mongoConfig.NomeBanco);
        });

        // Repositórios
        services.AddScoped<ILogProcessamentoLensRepository, LogProcessamentoLensRepository>();

        // Serviços
        services.AddScoped<IConstrivaLensService, ConstrivaLensService>();
        services.AddScoped<ILensAuthenticationService, LensAuthenticationService>();
        services.AddScoped<ISignalRNotificationService, SignalRNotificationService>();

        // Cache em memória
        services.AddMemoryCache();

        // HttpClient - Constriva.Lens com Polly
        var lensBaseUrl = configuration["ConstrivaLens:BaseUrl"] ?? "http://localhost:8001";
        var lensTimeout = int.Parse(configuration["ConstrivaLens:TimeoutSegundos"] ?? "120");

        services.AddHttpClient("ConstrivaLens", client =>
        {
            client.BaseAddress = new Uri(lensBaseUrl);
            client.Timeout = TimeSpan.FromSeconds(lensTimeout);
        })
        .AddPolicyHandler((sp, _) => RetryPolicy.Criar(sp))
        .AddPolicyHandler((sp, _) => CircuitBreakerPolicy.Criar(sp));

        // HttpClient - Constriva.API (para notificações SignalR)
        var apiBaseUrl = configuration["ConstrivaApi:BaseUrl"] ?? "http://localhost:5000";
        var chaveInterna = configuration["ConstrivaApi:ChaveInterna"] ?? "";

        services.AddHttpClient("ConstrivaApi", client =>
        {
            client.BaseAddress = new Uri(apiBaseUrl);
            client.DefaultRequestHeaders.Add("X-Constriva-Internal-Key", chaveInterna);
        })
        .ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
        {
            ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
        });

        // Health Checks
        services.AddHealthChecks()
            .AddCheck("rabbitmq", () => HealthCheckResult.Healthy("RabbitMQ operacional"), tags: new[] { "ready" })
            .AddCheck("mongodb", () =>
            {
                try
                {
                    var client = new MongoClient(mongoConfig.ConnectionString);
                    client.ListDatabaseNames();
                    return HealthCheckResult.Healthy("MongoDB operacional");
                }
                catch (Exception ex)
                {
                    return HealthCheckResult.Unhealthy("MongoDB indisponível", ex);
                }
            }, tags: new[] { "ready" });

        return services;
    }
}
