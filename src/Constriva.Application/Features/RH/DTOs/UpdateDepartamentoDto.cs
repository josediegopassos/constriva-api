namespace Constriva.Application.Features.RH.DTOs;

public record UpdateDepartamentoDto(
    string? Nome = null,
    string? Descricao = null,
    Guid? GestorId = null,
    Guid? DepartamentoPaiId = null,
    bool? Ativo = null);
