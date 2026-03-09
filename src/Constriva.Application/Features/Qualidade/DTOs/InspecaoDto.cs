using Constriva.Domain.Enums;

namespace Constriva.Application.Features.Qualidade.DTOs;

public record InspecaoDto(
    Guid Id, Guid ObraId, string Numero, string Titulo, string? Descricao,
    StatusInspecaoEnum Status, DateTime DataProgramada, DateTime? DataRealizacao,
    string? Localizacao, bool TemNaoConformidade, DateTime CreatedAt);
