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
    EnderecoDto? Endereco = null);
