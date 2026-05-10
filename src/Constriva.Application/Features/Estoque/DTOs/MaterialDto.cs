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
