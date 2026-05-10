namespace Constriva.Application.Features.Compras.DTOs;

public record CreateCotacaoDto(
    Guid ObraId,
    string Titulo,
    DateTime? DataLimiteResposta = null,
    string? Observacoes = null,
    string? CondicoesGerais = null,
    IEnumerable<CreateItemCotacaoDto>? Itens = null);
