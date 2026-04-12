namespace Constriva.Application.Features.RH.DTOs;

public record UpdatePontoDto(
    Guid? ObraId, DateTime DataHora, string? HorarioPrevisto, decimal? HorasExtras,
    string? Latitude, string? Longitude, string? Dispositivo,
    bool? Online, bool? Manual, string? Justificativa);
