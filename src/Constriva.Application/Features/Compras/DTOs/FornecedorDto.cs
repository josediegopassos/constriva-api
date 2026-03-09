using Constriva.Domain.Enums;

namespace Constriva.Application.Features.Compras.DTOs;

public record FornecedorDto(Guid Id, string RazaoSocial, string NomeFantasia,
    string? Cnpj, string? Cpf, TipoFornecedorEnum Tipo,
    string? Telefone, string? Email, string? Cidade, bool Ativo);
