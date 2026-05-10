namespace Constriva.Application.Features.Compras.DTOs;

public record PropostaDto(
    Guid Id,
    Guid CotacaoId,
    Guid FornecedorId,
    string FornecedorNome,
    DateTime DataRecebimento,
    DateTime? DataValidade,
    string? CondicoesPagamento,
    int? PrazoEntrega,
    string? Observacoes,
    decimal ValorTotal,
    bool Vencedora,
    IEnumerable<ItemPropostaDto> Itens);
