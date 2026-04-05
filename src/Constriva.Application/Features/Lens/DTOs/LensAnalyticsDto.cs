namespace Constriva.Application.Features.Lens.DTOs;

public record ResumoAnalyticsLensDto(
    int TotalDocumentos,
    int TotalSucesso,
    int TotalErro,
    float TaxaSucesso,
    float ConfidenceMedio,
    int TempoMedioProcessamentoMs,
    int TotalItensExtraidos,
    int TotalItensConsolidados,
    DateTime PeriodoDe,
    DateTime PeriodoAte);

public record AnalyticsPorTipoDto(
    string TipoDocumento,
    int Total,
    int Sucesso,
    int Erro,
    float ConfidenceMedio);

public record TendenciaConfidenceDto(
    DateTime Data,
    float ConfidenceMedio,
    int TotalDocumentos);

public record WarningFrequenteDto(
    string Warning,
    int Frequencia);
