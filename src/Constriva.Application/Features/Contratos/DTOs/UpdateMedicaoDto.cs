namespace Constriva.Application.Features.Contratos.DTOs;

public record UpdateMedicaoDto(string Numero, DateTime DataMedicao, decimal ValorMedido, string? Observacoes);
