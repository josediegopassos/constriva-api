namespace Constriva.Application.Features.RH.DTOs;

public record CreateCargoDto(
    string Nome,
    string? CBO = null,
    string? Descricao = null,
    decimal SalarioBase = 0,
    decimal? SalarioMaximo = null);
