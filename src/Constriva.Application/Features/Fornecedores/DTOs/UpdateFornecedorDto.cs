using Constriva.Domain.Enums;

namespace Constriva.Application.Features.Fornecedores.DTOs;

public record UpdateFornecedorDto(
    string? RazaoSocial = null,
    TipoPessoaEnum? TipoPessoa = null,
    string? NomeFantasia = null,
    string? Documento = null,
    string? InscricaoEstadual = null,
    string? Email = null,
    string? Telefone = null,
    string? Celular = null,
    string? Site = null,
    string? Contato = null,
    TipoFornecedorEnum? Tipo = null,
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
