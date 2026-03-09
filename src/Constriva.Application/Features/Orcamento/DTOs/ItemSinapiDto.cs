namespace Constriva.Application.Features.Orcamento.DTOs;

public record ItemSinapiDto(
    string Codigo,
    string Descricao,
    string UnidadeMedida,
    decimal CustoUnitario,
    decimal Quantidade = 1);
