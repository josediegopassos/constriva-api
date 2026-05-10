namespace Constriva.Application.Features.Agente.DTOs;

public record ConsumoResumoDto(
    long TokensUtilizados, long TokensLimite, decimal PercentualUso,
    long TokensRestantes, bool Alerta80Enviado);
