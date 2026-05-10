namespace Constriva.Application.Features.RH.DTOs;

public record CargoDto(Guid Id, string Codigo, string Nome, string? CBO, string? Descricao, decimal SalarioBase, decimal? SalarioMaximo, bool Ativo);
