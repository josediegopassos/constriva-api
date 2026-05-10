namespace Constriva.Application.Features.Lens.DTOs;

public record SugestaoMatchingDto(
    FornecedorSugeridoDto? FornecedorSugerido,
    ObraSugeridaDto? ObraSugerida,
    float ConfiancaMatchFornecedor);
