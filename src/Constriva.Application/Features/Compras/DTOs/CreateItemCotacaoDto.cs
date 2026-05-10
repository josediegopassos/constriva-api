namespace Constriva.Application.Features.Compras.DTOs;

public record CreateItemCotacaoDto(
    string Descricao,
    string UnidadeMedida,
    decimal Quantidade,
    Guid? MaterialId = null,
    string? Especificacao = null,
    decimal? PrecoReferencia = null);
