namespace Constriva.Application.Features.Compras.DTOs;

public record UpdateFornecedorDto(string RazaoSocial, string? NomeFantasia, string? CNPJ, string? Email, string? Telefone, string? Endereco, string Tipo);
