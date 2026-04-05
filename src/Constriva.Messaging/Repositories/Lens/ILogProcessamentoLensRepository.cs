using Constriva.Messaging.Models.Lens;

namespace Constriva.Messaging.Repositories.Lens;

/// <summary>
/// Repositório para operações de log de processamento Lens no MongoDB.
/// </summary>
public interface ILogProcessamentoLensRepository
{
    /// <summary>Insere um novo log de processamento.</summary>
    Task InserirAsync(LogProcessamentoLens log, CancellationToken ct);

    /// <summary>Obtém um log pelo identificador do processamento.</summary>
    Task<LogProcessamentoLens?> ObterPorProcessamentoIdAsync(Guid processamentoId, CancellationToken ct);

    /// <summary>Lista logs de uma empresa em um período.</summary>
    Task<List<LogProcessamentoLens>> ListarPorEmpresaAsync(Guid empresaId, DateTime de, DateTime ate, CancellationToken ct);

    /// <summary>Obtém resumo analytics de processamentos.</summary>
    Task<ResumoProcessamentoLens> ObterResumoAsync(Guid empresaId, DateTime de, DateTime ate, CancellationToken ct);

    /// <summary>Obtém distribuição de processamentos por tipo de documento.</summary>
    Task<List<ProcessamentoPorTipo>> ObterPorTipoAsync(Guid empresaId, DateTime de, DateTime ate, CancellationToken ct);

    /// <summary>Obtém tendência de confidence score ao longo do tempo.</summary>
    Task<List<TendenciaConfidence>> ObterTendenciaConfidenceAsync(Guid empresaId, DateTime de, DateTime ate, CancellationToken ct);

    /// <summary>Obtém os warnings mais frequentes.</summary>
    Task<List<WarningFrequente>> ObterWarningsFrequentesAsync(Guid empresaId, int limite, CancellationToken ct);
}

/// <summary>Resumo de processamentos para analytics.</summary>
public class ResumoProcessamentoLens
{
    public int TotalDocumentos { get; set; }
    public int TotalSucesso { get; set; }
    public int TotalErro { get; set; }
    public float TaxaSucesso { get; set; }
    public float ConfidenceMedio { get; set; }
    public int TempoMedioProcessamentoMs { get; set; }
    public int TotalItensExtraidos { get; set; }
}

/// <summary>Contagem de processamentos por tipo de documento.</summary>
public class ProcessamentoPorTipo
{
    public string TipoDocumento { get; set; } = string.Empty;
    public int Total { get; set; }
    public int Sucesso { get; set; }
    public int Erro { get; set; }
    public float ConfidenceMedio { get; set; }
}

/// <summary>Tendência de confidence score por dia.</summary>
public class TendenciaConfidence
{
    public DateTime Data { get; set; }
    public float ConfidenceMedio { get; set; }
    public int TotalDocumentos { get; set; }
}

/// <summary>Warning com frequência de ocorrência.</summary>
public class WarningFrequente
{
    public string Warning { get; set; } = string.Empty;
    public int Frequencia { get; set; }
}
