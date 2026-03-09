using Constriva.Domain.Enums;

namespace Constriva.Application.Features.Obras.DTOs;

public record ObraResumoDto(
    Guid Id, string Codigo, string Nome, TipoObraEnum Tipo,
    StatusObraEnum Status, string StatusLabel,
    string Cidade, string Estado,
    DateTime DataInicioPrevista, DateTime DataFimPrevista,
    decimal ValorContrato, decimal PercentualConcluido,
    bool Atrasada, string? FotoUrl);
