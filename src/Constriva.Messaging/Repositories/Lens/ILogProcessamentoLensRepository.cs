using Constriva.Messaging.Models.Lens;

namespace Constriva.Messaging.Repositories.Lens;

public interface ILogProcessamentoLensRepository
{
    Task InserirAsync(LogProcessamentoLens log, CancellationToken ct);
    Task<LogProcessamentoLens?> ObterPorProcessamentoIdAsync(Guid processamentoId, CancellationToken ct);
    Task<List<LogProcessamentoLens>> ListarPorEmpresaAsync(Guid empresaId, DateTime de, DateTime ate, CancellationToken ct);
    Task<ResumoProcessamentoLens> ObterResumoAsync(Guid empresaId, DateTime de, DateTime ate, CancellationToken ct);
    Task<List<ProcessamentoPorTipo>> ObterPorTipoAsync(Guid empresaId, DateTime de, DateTime ate, CancellationToken ct);
    Task<List<TendenciaConfidence>> ObterTendenciaConfidenceAsync(Guid empresaId, DateTime de, DateTime ate, CancellationToken ct);
    Task<List<WarningFrequente>> ObterWarningsFrequentesAsync(Guid empresaId, int limite, CancellationToken ct);
}

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

public class ProcessamentoPorTipo
{
    public string TipoDocumento { get; set; } = string.Empty;
    public int Total { get; set; }
    public int Sucesso { get; set; }
    public int Erro { get; set; }
    public float ConfidenceMedio { get; set; }
}

public class TendenciaConfidence
{
    public DateTime Data { get; set; }
    public float ConfidenceMedio { get; set; }
    public int TotalDocumentos { get; set; }
}

public class WarningFrequente
{
    public string Warning { get; set; } = string.Empty;
    public int Frequencia { get; set; }
}
