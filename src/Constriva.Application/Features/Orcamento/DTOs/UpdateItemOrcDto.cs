using Constriva.Domain.Enums;

namespace Constriva.Application.Features.Orcamento.DTOs;

public record UpdateItemOrcDto(
    string Descricao,
    string UnidadeMedida,
    decimal Quantidade,
    decimal CustoUnitario,
    FontePrecoEnum Fonte = FontePrecoEnum.Manual,
    string? CodigoFonte = null);
