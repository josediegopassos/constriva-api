namespace Constriva.Application.Features.Agente.DTOs;

public record AdminRelatorioItemDto(
    Guid EmpresaId, string EmpresaNome, string TierNome,
    long TokensLimite, long TokensUtilizados, decimal PercentualUso,
    long TokensAvulsosUtilizados, int TotalRequisicoes, decimal CustoEstimadoUsd);
