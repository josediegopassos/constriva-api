using Constriva.Domain.Enums;

namespace Constriva.Application.Features.RH.DTOs;

public record RegistroPontoDto(
    Guid Id, Guid FuncionarioId, string FuncionarioNome,
    TipoRegistroPontoEnum Tipo, DateTime DataHora, string? HorarioPrevisto,
    decimal? HorasExtras);
