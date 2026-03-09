namespace Constriva.Application.Features.Orcamento.DTOs;

public record CreateGrupoDto(
    string Nome,
    Guid? GrupoPaiId = null);
