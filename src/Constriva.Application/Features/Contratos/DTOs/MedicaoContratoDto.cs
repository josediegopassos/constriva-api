using Constriva.Domain.Enums;

namespace Constriva.Application.Features.Contratos.DTOs;

public record MedicaoContratoDto(
    Guid Id, Guid ContratoId, int Periodo, string Numero,
    decimal ValorMedicao, decimal ValorLiquido,
    StatusMedicaoEnum Status, DateTime DataInicio, DateTime DataFim,
    DateTime? DataSubmissao, DateTime? DataAprovacao);
