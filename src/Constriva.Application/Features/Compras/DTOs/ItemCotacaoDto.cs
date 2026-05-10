namespace Constriva.Application.Features.Compras.DTOs;

public record ItemCotacaoDto(
    Guid Id,
    Guid? MaterialId,
    string Descricao,
    string UnidadeMedida,
    decimal Quantidade,
    string? Especificacao,
    decimal? PrecoReferencia,
    int Ordem);
