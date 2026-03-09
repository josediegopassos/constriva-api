using Constriva.Domain.Enums;

namespace Constriva.Application.Features.Orcamento.DTOs;

public record CreateComposicaoDto(
    string Codigo,
    string Descricao,
    string UnidadeMedida,
    FontePrecoEnum Fonte = FontePrecoEnum.Manual,
    string? CodigoFonte = null,
    string? Observacoes = null);
