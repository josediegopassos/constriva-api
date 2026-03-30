using Constriva.Domain.Enums;

namespace Constriva.Application.Features.RH.DTOs;

public record FolhaPagamentoDto(
    Guid Id, string Competencia, DateTime DataInicio, DateTime DataFim,
    StatusFolhaPagamentoEnum Status, int TotalFuncionarios,
    decimal ValorTotalBruto, decimal ValorTotalDescontos, decimal ValorTotalLiquido,
    Guid? AprovadoPor, DateTime? DataAprovacao, DateTime? DataPagamento);
