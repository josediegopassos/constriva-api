namespace Constriva.Application.Features.RH.DTOs;

public record UpdateFuncionarioDto(string Nome, string Cargo, string? Departamento, decimal SalarioBase, string? Telefone, string? Email, bool Ativo, Guid? ObraId);
