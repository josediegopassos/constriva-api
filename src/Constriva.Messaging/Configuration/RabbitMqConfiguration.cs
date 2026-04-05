namespace Constriva.Messaging.Configuration;

/// <summary>
/// Configuração de conexão e comportamento do RabbitMQ.
/// </summary>
public class RabbitMqConfiguration
{
    /// <summary>Endereço do servidor RabbitMQ.</summary>
    public string Host { get; set; } = "localhost";

    /// <summary>Porta de conexão AMQP.</summary>
    public int Porta { get; set; } = 5672;

    /// <summary>Usuário de autenticação.</summary>
    public string Usuario { get; set; } = "guest";

    /// <summary>Senha de autenticação.</summary>
    public string Senha { get; set; } = "guest";

    /// <summary>Virtual host do RabbitMQ.</summary>
    public string VirtualHost { get; set; } = "/";

    /// <summary>Quantidade de mensagens pré-carregadas por consumer.</summary>
    public int PrefetchCount { get; set; } = 10;

    /// <summary>Limite de concorrência de processamento.</summary>
    public int LimiteConcorrencia { get; set; } = 5;
}
