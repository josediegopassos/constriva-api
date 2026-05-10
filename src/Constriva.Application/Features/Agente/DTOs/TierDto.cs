namespace Constriva.Application.Features.Agente.DTOs;

public record TierDto(Guid Id, string Nome, long TokensMensais, string? Descricao);
