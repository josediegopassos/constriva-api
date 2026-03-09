namespace Constriva.Application.Features.SST.DTOs;

public record IndicadoresDashboardDto(
    int TotalDDS, int TotalAcidentes, int AcidentesGraves,
    int TotalDiasAfastamento, decimal TaxaFrequencia, decimal TaxaGravidade,
    int DiasDesdeUltimoAcidente);
