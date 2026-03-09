namespace Constriva.Application.Features.Financeiro.DTOs;

public record DREDto(
    string Periodo, decimal ReceitaBruta, decimal Deducoes, decimal ReceitaLiquida,
    decimal CustosDiretos, decimal LucroBruto, decimal DespesasOperacionais,
    decimal Ebitda, decimal Depreciacao, decimal Ebit, decimal ResultadoFinanceiro,
    decimal LucroAnteIR, decimal IR, decimal LucroLiquido
);
