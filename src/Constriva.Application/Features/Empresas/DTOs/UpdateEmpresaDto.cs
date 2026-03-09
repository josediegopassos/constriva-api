namespace Constriva.Application.Features.Empresas.DTOs;

public record UpdateEmpresaDto(
    string RazaoSocial, string NomeFantasia, string Email, string Telefone,
    string? Site, string Logradouro, string Numero, string? Complemento,
    string Bairro, string Cidade, string Estado, string Cep
);
