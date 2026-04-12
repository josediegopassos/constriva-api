using Constriva.Domain.Enums;

namespace Constriva.Application.Features.RH.DTOs;

public record RegistroPontoDto(
    Guid Id, Guid FuncionarioId, string FuncionarioNome,
    Guid? ObraId, TipoRegistroPontoEnum Tipo, DateTime DataHora,
    string? HorarioPrevisto, decimal? HorasExtras,
    string? Latitude, string? Longitude, string? Dispositivo,
    bool Online, bool Manual, string? Justificativa,
    StatusAprovacaoPontoEnum StatusAprovacao, Guid? AprovadoPor, string? AprovadoPorNome,
    Guid? ReprovadoPor, string? ReprovadoPorNome,
    DateTime CreatedAt);
