namespace Constriva.Application.Features.Compras.DTOs;

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
