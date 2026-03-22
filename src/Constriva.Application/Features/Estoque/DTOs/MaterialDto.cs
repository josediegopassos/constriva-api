using Constriva.Domain.Enums;

namespace Constriva.Application.Features.Estoque.DTOs;

public record MaterialDto(
    Guid Id,
    string Codigo,
    string Nome,
    string UnidadeMedida,
    TipoInsumoEnum Tipo,
    string? CodigoSINAPI,
    string? Marca,
    string? Fabricante,
    Guid? GrupoId,
    string? GrupoNome,
    decimal EstoqueMinimo,
    decimal EstoqueMaximo,
    decimal PrecoCustoMedio,
    decimal PrecoUltimaCompra,
    bool Ativo,
    bool ControlaLote,
    bool ControlaValidade);

public record MaterialDetalheDto(
    Guid Id,
    string Codigo,
    string Nome,
    string? Descricao,
    string? Especificacao,
    string UnidadeMedida,
    string? CodigoBarras,
    string? CodigoSINAPI,
    string? Marca,
    string? Fabricante,
    TipoInsumoEnum Tipo,
    Guid? GrupoId,
    string? GrupoNome,
    decimal EstoqueMinimo,
    decimal EstoqueMaximo,
    decimal PrecoCustoMedio,
    decimal PrecoUltimaCompra,
    bool Ativo,
    string? ImagemUrl,
    string? Observacoes,
    bool ControlaLote,
    bool ControlaValidade,
    DateTime CreatedAt);
