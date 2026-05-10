namespace Constriva.Application.Features.Lens.DTOs;

public record AnalyticsPorTipoDto(
    string TipoDocumento,
    int Total,
    int Sucesso,
    int Erro,
    float ConfidenceMedio);
