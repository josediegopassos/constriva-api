using MassTransit;
using Constriva.Messaging.Consumers.Lens;
using Constriva.Messaging.Consumers.WhatsApp;

namespace Constriva.Messaging.Configuration;

public static class MassTransitConfiguration
{
    public static IServiceCollection AdicionarMassTransit(this IServiceCollection services, IConfiguration configuration)
    {
        var rabbitConfig = configuration.GetSection("RabbitMq").Get<RabbitMqConfiguration>() ?? new RabbitMqConfiguration();

        services.AddMassTransit(cfg =>
        {
            cfg.AddConsumer<ProcessarDocumentoLensConsumer>();
            cfg.AddConsumer<ReprocessarDocumentoLensConsumer>();

            cfg.AddConsumer<EnviarCotacaoWhatsAppConsumer>();
            cfg.AddConsumer<EnviarLembreteCotacaoConsumer>();
            cfg.AddConsumer<ProcessarRespostaFornecedorConsumer>();
            cfg.AddConsumer<CotacaoAprovadaConsumer>();

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

                rabbitCfg.ReceiveEndpoint("constriva-whatsapp-enviar-cotacao", e =>
                {
                    e.PrefetchCount = rabbitConfig.PrefetchCount;
                    e.ConcurrentMessageLimit = rabbitConfig.LimiteConcorrencia;
                    e.ConfigureConsumer<EnviarCotacaoWhatsAppConsumer>(context);
                });

                rabbitCfg.ReceiveEndpoint("constriva-whatsapp-enviar-lembrete", e =>
                {
                    e.PrefetchCount = 2;
                    e.ConcurrentMessageLimit = 2;
                    e.ConfigureConsumer<EnviarLembreteCotacaoConsumer>(context);
                });

                rabbitCfg.ReceiveEndpoint("constriva-whatsapp-processar-resposta", e =>
                {
                    e.PrefetchCount = 1;
                    e.ConcurrentMessageLimit = 1;
                    e.ConfigureConsumer<ProcessarRespostaFornecedorConsumer>(context);
                });

                rabbitCfg.ReceiveEndpoint("constriva-whatsapp-cotacao-aprovada", e =>
                {
                    e.PrefetchCount = 1;
                    e.ConcurrentMessageLimit = 1;
                    e.ConfigureConsumer<CotacaoAprovadaConsumer>(context);
                });
            });
        });

        return services;
    }
}
