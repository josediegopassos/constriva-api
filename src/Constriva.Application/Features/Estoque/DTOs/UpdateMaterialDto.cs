using Constriva.Domain.Enums;

namespace Constriva.Application.Features.Estoque.DTOs;

public record UpdateMaterialDto(
    string Nome,
    string UnidadeMedida,
    TipoInsumoEnum Tipo,
    string? Codigo,
    string? Descricao,
    string? Especificacao,
    string? CodigoBarras,
    string? CodigoSINAPI,
    string? Marca,
    string? Fabricante,
    Guid? GrupoId,
    decimal EstoqueMinimo,
    decimal EstoqueMaximo,
    decimal? PrecoCustoMedio,
    decimal? PrecoUltimaCompra,
    string? ImagemUrl,
    string? Observacoes,
    bool ControlaLote,
    bool ControlaValidade);
