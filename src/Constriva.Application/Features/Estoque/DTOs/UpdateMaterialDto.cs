namespace Constriva.Application.Features.Estoque.DTOs;

public record UpdateMaterialDto(string Nome, string Unidade, string? Categoria, string? CodigoInterno, decimal? PrecoUnitario);
