namespace Constriva.Application.Features.RH.DTOs;

public record CargoDto(Guid Id, string Nome, string? Descricao, decimal? SalarioBase, decimal? SalarioTeto);
