namespace Constriva.Application.Features.RH.DTOs;

public record DepartamentoDto(Guid Id, string Nome, string? Descricao, Guid? GestorId, string? GestorNome, Guid? DepartamentoPaiId, bool Ativo);
