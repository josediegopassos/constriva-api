namespace Constriva.Application.Features.Lens.DTOs;

public record SugestaoMatchingDto(
    FornecedorSugeridoDto? FornecedorSugerido,
    ObraSugeridaDto? ObraSugerida,
    float ConfiancaMatchFornecedor);

public record FornecedorSugeridoDto(
    Guid Id,
    string RazaoSocial,
    string? NomeFantasia,
    string Documento);

public record ObraSugeridaDto(
    Guid Id,
    string Nome);
