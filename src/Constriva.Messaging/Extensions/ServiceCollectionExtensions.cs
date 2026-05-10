using System.Net.Http.Headers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Constriva.Application.Common.Interfaces;
using Constriva.Domain.Interfaces.WhatsApp;
using Constriva.Infrastructure.Integrations.OpenAI.Extrator;
using Constriva.Infrastructure.Integrations.WhatsApp;
using Constriva.Infrastructure.Integrations.WhatsApp.Options;
using Constriva.Infrastructure.Persistence;
using Constriva.Infrastructure.Services;
using Constriva.Messaging.Configuration;
using Constriva.Messaging.Policies;
using Constriva.Messaging.Repositories.Lens;
using Constriva.Messaging.Services.Lens;
using Constriva.Messaging.Services.Notification;

namespace Constriva.Messaging.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AdicionarConstrivaMensageria(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<RabbitMqConfiguration>(configuration.GetSection("RabbitMq"));
        services.Configure<MongoDbConfiguration>(configuration.GetSection("MongoDb"));

        services.AdicionarMassTransit(configuration);

        var mongoConfig = configuration.GetSection("MongoDb").Get<MongoDbConfiguration>() ?? new MongoDbConfiguration();
        services.AddSingleton<IMongoClient>(sp => new MongoClient(mongoConfig.ConnectionString));
        services.AddSingleton(sp =>
        {
            var client = sp.GetRequiredService<IMongoClient>();
            return client.GetDatabase(mongoConfig.NomeBanco);
        });

        services.AddScoped<ILogProcessamentoLensRepository, LogProcessamentoLensRepository>();

        services.AddScoped<IConstrivaLensService, ConstrivaLensService>();
        services.AddScoped<ILensAuthenticationService, LensAuthenticationService>();
        services.AddScoped<ISignalRNotificationService, SignalRNotificationService>();

        services.AddMemoryCache();

        var lensBaseUrl = configuration["ConstrivaLens:BaseUrl"] ?? "http://localhost:8001";
        var lensTimeout = int.Parse(configuration["ConstrivaLens:TimeoutSegundos"] ?? "120");

        services.AddHttpClient("ConstrivaLens", client =>
        {
            client.BaseAddress = new Uri(lensBaseUrl);
            client.Timeout = TimeSpan.FromSeconds(lensTimeout);
        })
        .AddPolicyHandler((sp, _) => RetryPolicy.Criar(sp))
        .AddPolicyHandler((sp, _) => CircuitBreakerPolicy.Criar(sp));

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

        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        var connStr = configuration.GetConnectionString("DefaultConnection");
        if (!string.IsNullOrEmpty(connStr))
        {
            var dataSourceBuilder = new Npgsql.NpgsqlDataSourceBuilder(connStr);
            dataSourceBuilder.EnableDynamicJson();
            var dataSource = dataSourceBuilder.Build();

            services.AddDbContext<AppDbContext>(options =>
                options.UseNpgsql(dataSource,
                    npg => npg.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName)));
        }

        services.Configure<WhatsAppOptions>(configuration.GetSection("WhatsApp"));
        services.Configure<Constriva.Application.Features.Agente.Settings.OpenAISettings>(
            configuration.GetSection("OpenAI"));

        services.AddHttpClient("OpenAI", client =>
        {
            var baseUrl = configuration["OpenAI:BaseUrl"] ?? "https://api.openai.com";
            client.BaseAddress = new Uri(baseUrl);
            var apiKey = configuration["OpenAI:ApiKey"];
            if (!string.IsNullOrEmpty(apiKey))
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");
        });

        services.AddHttpClient("WhatsApp", (sp, client) =>
        {
            var opts = sp.GetRequiredService<IOptions<WhatsAppOptions>>().Value;
            client.BaseAddress = new Uri(opts.BaseUrl);
            client.DefaultRequestHeaders.Add("Accept", "application/json");
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", opts.AccessToken);
            client.Timeout = TimeSpan.FromSeconds(opts.TimeoutSegundos);
        });

        services.AddScoped<IWhatsAppGateway, WhatsAppGatewayService>();
        services.AddScoped<IExtratorPropostaService, ExtratorPropostaService>();
        services.AddScoped<IFileStorageService, S3StorageService>();

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
