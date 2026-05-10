namespace Constriva.Application.Features.Agente.DTOs;

public record ChatResponseDto(
    Guid SessaoId, string Resposta, int TokensConsumidos,
    long TokensRestantes, decimal PercentualUsado);
