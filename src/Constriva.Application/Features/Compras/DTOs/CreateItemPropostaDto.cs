namespace Constriva.Application.Features.Compras.DTOs;

public record CreateItemPropostaDto(
    Guid ItemCotacaoId,
    decimal PrecoUnitario,
    decimal Quantidade,
    string? Marca = null,
    string? Observacao = null,
    bool Disponivel = true);
