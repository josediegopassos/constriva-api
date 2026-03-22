namespace Constriva.Application.Features.Estoque.DTOs;

public record AlmoxarifadoDto(
    Guid Id,
    string Nome,
    string Codigo,
    Guid? ObraId,
    bool Principal,
    string? Descricao,
    string? Logradouro,
    string? Cidade,
    Guid? ResponsavelId,
    bool Ativo);
