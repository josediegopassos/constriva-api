using Constriva.Application.Features.Clientes.DTOs;
using Constriva.Domain.Enums;

namespace Constriva.Application.Features.RH.DTOs;

public record CreateFuncionarioDto(
    string Nome,
    string Cpf,
    string Email,
    DateTime DataAdmissao,
    decimal SalarioBase,
    string? NomeSocial = null,
    DateTime? DataNascimento = null,
    string? Rg = null,
    string? OrgaoExpedidor = null,
    string? Cnh = null,
    string? CategoriaCnh = null,
    DateTime? ValidadeCnh = null,
    string? CtpsNumero = null,
    string? CtpsSerie = null,
    string? Pis = null,
    string? Telefone = null,
    string? Celular = null,
    string? FotoUrl = null,
    string? Genero = null,
    string? EstadoCivil = null,
    string? Escolaridade = null,
    string? Naturalidade = null,
    string? Nacionalidade = null,
    TipoContratacaoEnum? TipoContratacao = null,
    Guid? CargoId = null,
    Guid? DepartamentoId = null,
    Guid? ObraId = null,
    decimal? HoraExtra50 = null,
    decimal? HoraExtra100 = null,
    int? JornadaDiaria = null,
    StatusFuncionarioEnum Status = StatusFuncionarioEnum.Ativo,
    EnderecoDto? Endereco = null,
    DadosBancariosDto? DadosBancarios = null);
