namespace Constriva.Messaging.Configuration;

/// <summary>
/// Configuração de conexão e comportamento do MongoDB.
/// </summary>
public class MongoDbConfiguration
{
    /// <summary>String de conexão do MongoDB.</summary>
    public string ConnectionString { get; set; } = "mongodb://localhost:27017";

    /// <summary>Nome do banco de dados.</summary>
    public string NomeBanco { get; set; } = "constriva_lens_logs";

    /// <summary>Nome da coleção de logs de processamento.</summary>
    public string NomeColecaoLogs { get; set; } = "log_processamento_lens";

    /// <summary>Dias de retenção dos logs (TTL).</summary>
    public int TtlDiasRetencao { get; set; } = 90;
}
