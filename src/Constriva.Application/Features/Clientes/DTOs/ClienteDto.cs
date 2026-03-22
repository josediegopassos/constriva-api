using Constriva.Domain.Enums;

namespace Constriva.Application.Features.Clientes.DTOs;

public record ClienteDto(
    Guid Id,
    string Codigo,
    TipoPessoaEnum TipoPessoa,
    string Nome,
    string? NomeFantasia,
    string? Documento,
    string? InscricaoEstadual,
    string? InscricaoMunicipal,
    string? Email,
    string? Telefone,
    string? Celular,
    string? Site,
    StatusClienteEnum Status,
    string? Observacoes,
    string? Logradouro,
    string? Numero,
    string? Complemento,
    string? Bairro,
    string? Cidade,
    string? Estado,
    string? Cep,
    DateTime CreatedAt);

public record ClienteAtivoDto(Guid Id, string Nome);

public record ClienteResumoDto(
    Guid Id,
    string Codigo,
    TipoPessoaEnum TipoPessoa,
    string Nome,
    string? NomeFantasia,
    string? Documento,
    string? Email,
    string? Telefone,
    StatusClienteEnum Status,
    string? Cidade,
    string? Estado);
