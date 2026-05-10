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
