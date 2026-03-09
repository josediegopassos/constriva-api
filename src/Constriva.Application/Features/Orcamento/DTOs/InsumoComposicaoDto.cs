using Constriva.Domain.Enums;

namespace Constriva.Application.Features.Orcamento.DTOs;

public record InsumoComposicaoDto(
    Guid Id,
    string Codigo,
    string Descricao,
    TipoInsumoEnum Tipo,
    string UnidadeMedida,
    decimal Coeficiente,
    decimal PrecoUnitario,
    decimal CustoTotal,
    FontePrecoEnum FontePrecoEnum);
