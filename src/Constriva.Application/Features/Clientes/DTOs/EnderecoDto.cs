namespace Constriva.Application.Features.Clientes.DTOs;

public record EnderecoDto(
    string? Logradouro,
    string? Numero,
    string? Complemento,
    string? Bairro,
    string? Cidade,
    string? Estado,
    string? Cep);
