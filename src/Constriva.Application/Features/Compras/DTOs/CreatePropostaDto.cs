namespace Constriva.Application.Features.Compras.DTOs;

public record CreatePropostaDto(
    Guid FornecedorId,
    DateTime? DataValidade = null,
    string? CondicoesPagamento = null,
    int? PrazoEntrega = null,
    string? Observacoes = null,
    IEnumerable<CreateItemPropostaDto>? Itens = null);
