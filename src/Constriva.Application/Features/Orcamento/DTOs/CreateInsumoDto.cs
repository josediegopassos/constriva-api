using Constriva.Domain.Enums;

namespace Constriva.Application.Features.Orcamento.DTOs;

public record CreateInsumoDto(
    string Codigo,
    string Descricao,
    TipoInsumoEnum Tipo,
    string UnidadeMedida,
    decimal Coeficiente,
    decimal PrecoUnitario,
    FontePrecoEnum FontePrecoEnum = FontePrecoEnum.Manual,
    Guid? MaterialId = null);
