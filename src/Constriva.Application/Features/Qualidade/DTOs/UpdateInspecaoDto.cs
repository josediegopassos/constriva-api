using Constriva.Domain.Enums;

namespace Constriva.Application.Features.Qualidade.DTOs;

public record UpdateInspecaoDto(string NumeroInspecao, DateTime DataProgramada, DateTime? DataRealizada, string? Responsavel, string? Observacoes, StatusInspecaoEnum Status);
