namespace Constriva.Application.Features.Cronograma.DTOs;

public record EVMDto(
    decimal ValorPlanejado, decimal ValorAgregado, decimal CustoReal,
    decimal VariacaoCusto, decimal VariacaoPrazo,
    decimal IndiceCusto, decimal IndicePrazo,
    decimal EstimativaTermino, decimal VariacaoTermino
);
