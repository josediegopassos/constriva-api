using Constriva.Domain.Enums;

namespace Constriva.Application.Features.Compras.DTOs;

public record CotacaoDto(
    Guid Id,
    string Numero,
    string Titulo,
    Guid ObraId,
    string? ObraNome,
    StatusCotacaoEnum Status,
    DateTime DataAbertura,
    DateTime? DataFechamento,
    DateTime? DataLimiteResposta,
    string? Observacoes,
    string? CondicoesGerais,
    Guid? FornecedorVencedorId,
    int TotalFornecedoresConvidados,
    int TotalPropostasRecebidas,
    DateTime CreatedAt,
    IEnumerable<ItemCotacaoDto> Itens);

public record ItemCotacaoDto(
    Guid Id,
    Guid? MaterialId,
    string Descricao,
    string UnidadeMedida,
    decimal Quantidade,
    string? Especificacao,
    decimal? PrecoReferencia,
    int Ordem);

public record CreateCotacaoDto(
    Guid ObraId,
    string Titulo,
    DateTime? DataLimiteResposta = null,
    string? Observacoes = null,
    string? CondicoesGerais = null,
    IEnumerable<CreateItemCotacaoDto>? Itens = null);

public record CreateItemCotacaoDto(
    string Descricao,
    string UnidadeMedida,
    decimal Quantidade,
    Guid? MaterialId = null,
    string? Especificacao = null,
    decimal? PrecoReferencia = null);

public record UpdateCotacaoDto(
    Guid? ObraId = null,
    string? Titulo = null,
    StatusCotacaoEnum? Status = null,
    DateTime? DataFechamento = null,
    DateTime? DataLimiteResposta = null,
    string? Observacoes = null,
    string? CondicoesGerais = null,
    Guid? FornecedorVencedorId = null,
    IEnumerable<CreateItemCotacaoDto>? Itens = null);
