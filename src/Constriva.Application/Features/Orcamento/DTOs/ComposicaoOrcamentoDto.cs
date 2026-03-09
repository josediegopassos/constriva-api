using Constriva.Domain.Enums;

namespace Constriva.Application.Features.Orcamento.DTOs;

public record ComposicaoOrcamentoDto(
    Guid Id,
    string Codigo,
    string Descricao,
    string UnidadeMedida,
    FontePrecoEnum Fonte,
    string? CodigoFonte,
    decimal CustoTotal,
    string? Observacoes,
    IEnumerable<InsumoComposicaoDto> Insumos);
