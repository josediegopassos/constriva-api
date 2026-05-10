using Constriva.Application.Features.Clientes.DTOs;
using Constriva.Domain.Enums;

namespace Constriva.Application.Features.RH.DTOs;

public record FuncionarioDetalhadoDto(
    Guid Id, string Matricula, string Nome, string? NomeSocial,
    DateTime DataNascimento, string Cpf, string? Rg, string? OrgaoExpedidor,
    string? Cnh, string? CategoriaCnh, DateTime? ValidadeCnh,
    string? CtpsNumero, string? CtpsSerie, string? Pis,
    string Email, string? Telefone, string? Celular,
    string? FotoUrl,
    string? Genero, string? EstadoCivil, string? Escolaridade,
    string? Naturalidade, string? Nacionalidade,
    EnderecoDto? Endereco,
    TipoContratacaoEnum TipoContratacao, StatusFuncionarioEnum Status,
    Guid? CargoId, string? CargoNome,
    Guid? DepartamentoId, string? DepartamentoNome,
    Guid? ObraAtualId, string? ObraNome,
    DateTime DataAdmissao, DateTime? DataDemissao, string? MotivoDemissao,
    decimal SalarioBase, decimal? HoraExtra50, decimal? HoraExtra100,
    int JornadaDiaria,
    DadosBancariosDto? DadosBancarios);
