namespace Constriva.Application.Features.Lens.DTOs;

public record TendenciaConfidenceDto(
    DateTime Data,
    float ConfidenceMedio,
    int TotalDocumentos);
