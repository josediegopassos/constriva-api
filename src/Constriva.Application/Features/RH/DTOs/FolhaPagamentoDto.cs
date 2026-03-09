namespace Constriva.Application.Features.RH.DTOs;

public record FolhaPagamentoDto(
    Guid Id, string Competencia, Guid FuncionarioId, string FuncionarioNome,
    decimal SalarioBase, decimal TotalAdicional, decimal TotalDesconto, decimal SalarioLiquido);
