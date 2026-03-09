using Constriva.Domain.Enums;

namespace Constriva.Application.Features.Orcamento.DTOs;

public record ItemOrcamentoDto(
    Guid Id,
    string Codigo,
    string Descricao,
    FontePrecoEnum Fonte,
    string? CodigoFonte,
    string UnidadeMedida,
    decimal Quantidade,
    decimal CustoUnitario,
    decimal CustoTotal,
    decimal BDI,
    decimal CustoComBDI,
    int Ordem,
    Guid? ComposicaoId);
