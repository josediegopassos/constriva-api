namespace Constriva.Application.Features.Estoque.DTOs;

public record CreateMaterialDto(string Nome, string Unidade, string? Categoria, string? CodigoInterno, decimal? PrecoUnitario);
