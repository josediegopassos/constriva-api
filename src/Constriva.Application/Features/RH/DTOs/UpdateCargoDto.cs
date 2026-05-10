namespace Constriva.Application.Features.RH.DTOs;

public record UpdateCargoDto(
    string? Nome = null,
    string? CBO = null,
    string? Descricao = null,
    decimal? SalarioBase = null,
    decimal? SalarioMaximo = null,
    bool? Ativo = null);
