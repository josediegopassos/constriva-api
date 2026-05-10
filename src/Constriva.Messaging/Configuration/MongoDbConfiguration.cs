namespace Constriva.Messaging.Configuration;

public class MongoDbConfiguration
{
    public string ConnectionString { get; set; } = "mongodb://localhost:27017";
    public string NomeBanco { get; set; } = "constriva_lens_logs";
    public string NomeColecaoLogs { get; set; } = "log_processamento_lens";
    public int TtlDiasRetencao { get; set; } = 90;
}
