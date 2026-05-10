namespace Constriva.Application.Features.RH.DTOs;

public record CreateDepartamentoDto(
    string Nome,
    string? Descricao = null,
    Guid? GestorId = null,
    Guid? DepartamentoPaiId = null);
