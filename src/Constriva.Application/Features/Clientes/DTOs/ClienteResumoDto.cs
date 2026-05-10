using Constriva.Domain.Enums;

namespace Constriva.Application.Features.Clientes.DTOs;

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
