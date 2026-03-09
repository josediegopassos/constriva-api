using Constriva.Domain.Enums;

namespace Constriva.Application.Features.Compras.DTOs;

public record CotacaoDto(Guid Id, string Numero, Guid ObraId,
    DateTime DataValidade, decimal ValorTotal, StatusCotacaoEnum Status);
