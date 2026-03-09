using Constriva.Domain.Enums;

namespace Constriva.Application.Features.RH.DTOs;

public record CreateFuncionarioDto(
    string Nome, string Cpf, string? Email, string? Telefone,
    Guid? CargoId, Guid? DepartamentoId,
    DateTime DataAdmissao, decimal SalarioBase, StatusFuncionarioEnum Status);
