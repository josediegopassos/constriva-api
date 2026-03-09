namespace Constriva.Application.Features.Contratos.DTOs;

public record MedicaoContratualDto(
    Guid Id, Guid ContratoId, string Numero, DateTime DataInicio,
    decimal ValorMedicao, string? Observacoes, DateTime CreatedAt);
