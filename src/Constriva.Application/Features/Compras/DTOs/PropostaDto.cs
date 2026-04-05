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

public record ItemPropostaDto(
    Guid Id,
    Guid ItemCotacaoId,
    string DescricaoItemCotacao,
    decimal PrecoUnitario,
    decimal Quantidade,
    decimal ValorTotal,
    string? Marca,
    string? Observacao,
    bool Disponivel,
    bool MenorPreco);

public record CreatePropostaDto(
    Guid FornecedorId,
    DateTime? DataValidade = null,
    string? CondicoesPagamento = null,
    int? PrazoEntrega = null,
    string? Observacoes = null,
    IEnumerable<CreateItemPropostaDto>? Itens = null);

public record CreateItemPropostaDto(
    Guid ItemCotacaoId,
    decimal PrecoUnitario,
    decimal Quantidade,
    string? Marca = null,
    string? Observacao = null,
    bool Disponivel = true);

public record UpdatePropostaDto(
    DateTime? DataValidade = null,
    string? CondicoesPagamento = null,
    int? PrazoEntrega = null,
    string? Observacoes = null,
    IEnumerable<CreateItemPropostaDto>? Itens = null);
