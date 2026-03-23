using Constriva.Domain.Enums;

namespace Constriva.Application.Features.Fornecedores.DTOs;

public record CreateFornecedorDto(
    TipoPessoaEnum TipoPessoa,
    string RazaoSocial,
    string Documento,
    string Email,
    TipoFornecedorEnum Tipo,
    string? NomeFantasia = null,
    string? InscricaoEstadual = null,
    string? Telefone = null,
    string? Celular = null,
    string? Site = null,
    string? Contato = null,
    string? Classificacao = null,
    int? Prazo = null,
    string? Observacoes = null,
    string? BancoNome = null,
    string? BancoAgencia = null,
    string? BancoConta = null,
    string? PixChave = null,
    string? Logradouro = null,
    string? Numero = null,
    string? Complemento = null,
    string? Bairro = null,
    string? Cidade = null,
    string? Estado = null,
    string? Cep = null);
