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
    string? FornecedorVencedorNome,
    int TotalFornecedoresConvidados,
    int TotalPropostasRecebidas,
    DateTime CreatedAt,
    IEnumerable<ItemCotacaoDto> Itens);
