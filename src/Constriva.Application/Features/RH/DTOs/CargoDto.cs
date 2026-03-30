namespace Constriva.Application.Features.RH.DTOs;

public record CargoDto(Guid Id, string Codigo, string Nome, string? CBO, string? Descricao, decimal SalarioBase, decimal? SalarioMaximo, bool Ativo);

public record CreateCargoDto(
    string Nome,
    string? CBO = null,
    string? Descricao = null,
    decimal SalarioBase = 0,
    decimal? SalarioMaximo = null);

public record UpdateCargoDto(
    string? Nome = null,
    string? CBO = null,
    string? Descricao = null,
    decimal? SalarioBase = null,
    decimal? SalarioMaximo = null,
    bool? Ativo = null);

public record ToggleAtivoDto(bool Ativo);
