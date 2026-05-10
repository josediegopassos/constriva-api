namespace Constriva.Application.Features.Lens.DTOs;

public record FornecedorSugeridoDto(
    Guid Id,
    string RazaoSocial,
    string? NomeFantasia,
    string Documento);
