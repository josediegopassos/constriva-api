namespace Constriva.Application.Features.RH.DTOs;

public record DepartamentoDto(Guid Id, string Nome, string? Descricao, Guid? GestorId, string? GestorNome, Guid? DepartamentoPaiId, bool Ativo);

public record CreateDepartamentoDto(
    string Nome,
    string? Descricao = null,
    Guid? GestorId = null,
    Guid? DepartamentoPaiId = null);

public record UpdateDepartamentoDto(
    string? Nome = null,
    string? Descricao = null,
    Guid? GestorId = null,
    Guid? DepartamentoPaiId = null,
    bool? Ativo = null);
