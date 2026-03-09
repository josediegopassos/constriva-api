using Constriva.Domain.Enums;

namespace Constriva.Application.Features.Empresas.DTOs;

public record EmpresaDto(
    Guid Id, string RazaoSocial, string NomeFantasia, string Cnpj, string Email, string Telefone,
    string? LogoUrl, StatusEmpresaEnum Status, PlanoEmpresaEnum Plano, DateTime? DataVencimento,
    int MaxUsuarios, int MaxObras, bool Ativo,
    string Cidade, string Estado,
    ModulosConfigDto Modulos, int TotalUsuarios, int TotalObras, DateTime CreatedAt
);
