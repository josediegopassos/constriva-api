using Constriva.Domain.Enums;

namespace Constriva.Application.Features.Lens.DTOs;

public record ItemDocumentoLensDto(
    Guid Id,
    int OrdemItem,
    string? Codigo,
    string Descricao,
    string? Ncm,
    string? Unidade,
    decimal? Quantidade,
    decimal? PrecoUnitario,
    decimal? PrecoTotal,
    decimal? Desconto,
    StatusItemLensEnum Status,
    string StatusDescricao,
    string? MotivoRejeicao,
    bool FoiEditado,
    string? DescricaoOriginal,
    decimal? QuantidadeOriginal,
    decimal? PrecoUnitarioOriginal,
    ProdutoSugeridoDto? ProdutoSugerido);

public record ProdutoSugeridoDto(
    Guid Id,
    string Codigo,
    string Descricao,
    string? Unidade);
