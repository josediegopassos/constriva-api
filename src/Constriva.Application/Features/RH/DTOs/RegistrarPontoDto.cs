using Constriva.Domain.Enums;

namespace Constriva.Application.Features.RH.DTOs;

public record RegistrarPontoDto(
    Guid FuncionarioId, TipoRegistroPontoEnum Tipo, DateTime DataHora, string? HorarioPrevisto);
