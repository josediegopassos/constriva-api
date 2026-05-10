namespace Constriva.Application.Features.Compras.DTOs;

public record UpdatePropostaDto(
    DateTime? DataValidade = null,
    string? CondicoesPagamento = null,
    int? PrazoEntrega = null,
    string? Observacoes = null,
    IEnumerable<CreateItemPropostaDto>? Itens = null);
