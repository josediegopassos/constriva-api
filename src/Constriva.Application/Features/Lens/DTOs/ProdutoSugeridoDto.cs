namespace Constriva.Application.Features.Lens.DTOs;

public record ProdutoSugeridoDto(
    Guid Id,
    string Codigo,
    string Descricao,
    string? Unidade);
