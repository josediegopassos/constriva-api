using Constriva.Domain.Enums;

namespace Constriva.Application.Features.RH.DTOs;

public record RegistrarPontoDto(
    Guid FuncionarioId, Guid? ObraId, TipoRegistroPontoEnum Tipo, DateTime DataHora,
    string? HorarioPrevisto, decimal? HorasExtras,
    string? Latitude, string? Longitude, string? Dispositivo,
    bool Online = true, bool Manual = false, string? Justificativa = null);
