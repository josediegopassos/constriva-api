using Constriva.Domain.Enums;

namespace Constriva.Application.Features.Cronograma.DTOs;

public record VinculoAtividadeDto(
    Guid Id, Guid AtividadeOrigemId, Guid AtividadeDestinoId,
    TipoVinculoEnum Tipo, int Lag
);
