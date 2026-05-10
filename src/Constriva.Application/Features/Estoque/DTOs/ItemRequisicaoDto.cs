namespace Constriva.Application.Features.Estoque.DTOs;

public record ItemRequisicaoDto(
    Guid Id,
    Guid MaterialId,
    string MaterialCodigo,
    string MaterialNome,
    string MaterialUnidade,
    decimal QuantidadeSolicitada,
    decimal QuantidadeAtendida,
    decimal? PrecoReferencia,
    string? Observacao);
