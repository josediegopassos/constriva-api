namespace Constriva.Messaging.Configuration;

public class RabbitMqConfiguration
{
    public string Host { get; set; } = "localhost";
    public int Porta { get; set; } = 5672;
    public string Usuario { get; set; } = "guest";
    public string Senha { get; set; } = "guest";
    public string VirtualHost { get; set; } = "/";
    public int PrefetchCount { get; set; } = 10;
    public int LimiteConcorrencia { get; set; } = 5;
}
