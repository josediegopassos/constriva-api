using Constriva.Domain.Enums;

namespace Constriva.Application.Features.Compras.DTOs;

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
