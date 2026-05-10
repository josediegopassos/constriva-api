using Constriva.Domain.Enums;

namespace Constriva.Application.Features.Fornecedores.DTOs;

public record FornecedorResumoDto(
    Guid Id,
    string Codigo,
    TipoPessoaEnum TipoPessoa,
    string RazaoSocial,
    string? NomeFantasia,
    string Documento,
    string? Email,
    string? Telefone,
    string? Celular,
    TipoFornecedorEnum Tipo,
    bool Ativo,
    bool Homologado,
    string? Cidade,
    string? Estado);
