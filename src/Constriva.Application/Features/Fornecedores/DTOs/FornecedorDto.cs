using Constriva.Domain.Enums;

namespace Constriva.Application.Features.Fornecedores.DTOs;

public record FornecedorDto(
    Guid Id,
    string Codigo,
    TipoPessoaEnum TipoPessoa,
    string RazaoSocial,
    string? NomeFantasia,
    string Documento,
    string? InscricaoEstadual,
    string Email,
    string? Telefone,
    string? Celular,
    string? Site,
    string? Contato,
    TipoFornecedorEnum Tipo,
    bool Ativo,
    bool Homologado,
    string? Classificacao,
    int? Prazo,
    string? Observacoes,
    string? BancoNome,
    string? BancoAgencia,
    string? BancoConta,
    string? PixChave,
    string? Logradouro,
    string? Numero,
    string? Complemento,
    string? Bairro,
    string? Cidade,
    string? Estado,
    string? Cep,
    DateTime CreatedAt);

public record FornecedorResumoDto(
    Guid Id,
    string Codigo,
    TipoPessoaEnum TipoPessoa,
    string RazaoSocial,
    string? NomeFantasia,
    string Documento,
    string? Email,
    string? Telefone,
    TipoFornecedorEnum Tipo,
    bool Ativo,
    bool Homologado,
    string? Cidade,
    string? Estado);

public record FornecedorAtivoDto(Guid Id, string RazaoSocial, string? NomeFantasia);
