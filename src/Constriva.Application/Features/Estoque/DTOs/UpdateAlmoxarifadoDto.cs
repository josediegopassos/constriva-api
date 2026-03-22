namespace Constriva.Application.Features.Estoque.DTOs;

public record UpdateAlmoxarifadoDto(
    string Nome,
    string? Descricao,
    string? Logradouro,
    string? Cidade,
    Guid? ResponsavelId,
    bool Principal);
