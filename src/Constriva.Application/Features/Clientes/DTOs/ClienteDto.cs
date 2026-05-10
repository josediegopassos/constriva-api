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
    EnderecoDto? Endereco,
    DateTime CreatedAt);
