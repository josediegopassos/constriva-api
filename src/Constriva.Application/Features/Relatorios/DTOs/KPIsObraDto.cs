namespace Constriva.Application.Features.Relatorios.DTOs;

public record KPIsObraDto(
    Guid ObraId, string ObraNome,
    decimal PercentualPlanejado, decimal PercentualRealizado,
    int AtividadesTotal, int AtividadesConcluidas, int AtividadesAtrasadas,
    decimal ValorContrato, decimal ValorMedidoAcumulado, decimal SaldoContrato,
    decimal CustoRealAcumulado, decimal VariacaoCusto,
    decimal VP, decimal VA, decimal CR, decimal VC, decimal IDP, decimal IDC);
