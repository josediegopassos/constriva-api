using Constriva.Domain.Enums;

namespace Constriva.Application.Features.Contratos.DTOs;

public record UpdateContratoDto(
    string Numero, string Objeto, decimal ValorTotal, DateTime DataInicio, DateTime DataFim,
    StatusContratoEnum Status, string? Observacoes);
