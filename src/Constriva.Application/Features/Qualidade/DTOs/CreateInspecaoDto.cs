namespace Constriva.Application.Features.Qualidade.DTOs;

public record CreateInspecaoDto(
    Guid ObraId, string Numero, string Titulo, string? Descricao,
    DateTime DataProgramada, string? Localizacao, Guid? InspetorId);
