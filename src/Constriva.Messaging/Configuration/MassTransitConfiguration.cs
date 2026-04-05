using MassTransit;
using Constriva.Messaging.Consumers.Lens;

namespace Constriva.Messaging.Configuration;

/// <summary>
/// Configuração do MassTransit com RabbitMQ para mensageria assíncrona.
/// </summary>
public static class MassTransitConfiguration
{
    /// <summary>
    /// Registra e configura o MassTransit com RabbitMQ no container de DI.
    /// </summary>
    public static IServiceCollection AdicionarMassTransit(this IServiceCollection services, IConfiguration configuration)
    {
        var rabbitConfig = configuration.GetSection("RabbitMq").Get<RabbitMqConfiguration>() ?? new RabbitMqConfiguration();

        services.AddMassTransit(cfg =>
        {
            cfg.AddConsumer<ProcessarDocumentoLensConsumer>();
            cfg.AddConsumer<ReprocessarDocumentoLensConsumer>();

            cfg.UsingRabbitMq((context, rabbitCfg) =>
            {
                rabbitCfg.Host(rabbitConfig.Host, (ushort)rabbitConfig.Porta, rabbitConfig.VirtualHost, h =>
                {
                    h.Username(rabbitConfig.Usuario);
                    h.Password(rabbitConfig.Senha);
                });

                rabbitCfg.UseMessageRetry(r =>
                {
                    r.Exponential(3, TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(30), TimeSpan.FromSeconds(5));
                });

                rabbitCfg.ReceiveEndpoint("constriva-lens-processar", e =>
                {
                    e.PrefetchCount = rabbitConfig.PrefetchCount;
                    e.ConcurrentMessageLimit = rabbitConfig.LimiteConcorrencia;
                    e.ConfigureConsumer<ProcessarDocumentoLensConsumer>(context);
                });

                rabbitCfg.ReceiveEndpoint("constriva-lens-reprocessar", e =>
                {
                    e.PrefetchCount = rabbitConfig.PrefetchCount;
                    e.ConcurrentMessageLimit = rabbitConfig.LimiteConcorrencia;
                    e.ConfigureConsumer<ReprocessarDocumentoLensConsumer>(context);
                });
            });
        });

        return services;
    }
}
