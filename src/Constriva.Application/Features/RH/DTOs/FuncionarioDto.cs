using Constriva.Domain.Enums;

namespace Constriva.Application.Features.RH.DTOs;

public record FuncionarioDto(
    Guid Id, string Matricula, string Nome, string? NomeSocial, string Cpf, string? Email, string? Telefone,
    Guid? CargoId, string? CargoNome, Guid? DepartamentoId, string? DepartamentoNome,
    Guid? ObraAtualId, string? ObraNome,
    TipoContratacaoEnum TipoContratacao, DateTime DataAdmissao, decimal SalarioBase, StatusFuncionarioEnum Status);
