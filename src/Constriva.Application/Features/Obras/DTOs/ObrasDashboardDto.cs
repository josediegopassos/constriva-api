namespace Constriva.Application.Features.Obras.DTOs;

public record ObrasDashboardDto(
    int TotalObras, int EmAndamento, int Concluidas, int Paralisadas,
    decimal ValorTotalContratado, decimal MediaAvanco,
    int ObrasAtrasadas,
    IEnumerable<ObraResumoDto> UltimasObras);
