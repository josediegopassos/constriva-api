using Constriva.Domain.Enums;

namespace Constriva.Application.Features.Empresas.DTOs;

public record CreateEmpresaDto(
    string RazaoSocial, string NomeFantasia, string Cnpj, string Email, string Telefone,
    string Logradouro, string Numero, string? Complemento, string Bairro, string Cidade, string Estado, string Cep,
    PlanoEmpresaEnum Plano, int MaxUsuarios, int MaxObras,
    string NomeAdmin, string EmailAdmin, string SenhaAdmin
);
