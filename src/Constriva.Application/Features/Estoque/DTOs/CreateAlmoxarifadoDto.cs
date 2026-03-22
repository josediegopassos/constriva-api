namespace Constriva.Application.Features.Estoque.DTOs;

public record CreateAlmoxarifadoDto(
    string Nome,
    Guid? ObraId,
    string? Descricao,
    string? Logradouro,
    string? Cidade,
    Guid? ResponsavelId,
    bool Principal);
