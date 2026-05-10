using Constriva.Domain.Enums;

namespace Constriva.Application.Features.RH.DTOs;

public record AlterarStatusFuncionarioDto(StatusFuncionarioEnum Status, string? Motivo = null);
