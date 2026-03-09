using Constriva.Domain.Enums;

namespace Constriva.Application.Features.RH.DTOs;

public record FuncionarioDto(
    Guid Id, string Nome, string Cpf, string? Email, string? Telefone,
    Guid? CargoId, string? CargoNome, Guid? DepartamentoId, string? DepartamentoNome,
    DateTime DataAdmissao, decimal SalarioBase, StatusFuncionarioEnum Status);
