using Constriva.Domain.Enums;

namespace Constriva.Application.Features.Clientes.DTOs;

public record CreateClienteDto(
    TipoPessoaEnum TipoPessoa,
    string Nome,
    string? NomeFantasia = null,
    string? Documento = null,
    string? InscricaoEstadual = null,
    string? InscricaoMunicipal = null,
    string? Email = null,
    string? Telefone = null,
    string? Celular = null,
    string? Site = null,
    string? Observacoes = null,
    string? Logradouro = null,
    string? Numero = null,
    string? Complemento = null,
    string? Bairro = null,
    string? Cidade = null,
    string? Estado = null,
    string? Cep = null);
