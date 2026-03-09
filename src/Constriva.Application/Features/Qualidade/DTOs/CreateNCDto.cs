using Constriva.Domain.Enums;

namespace Constriva.Application.Features.Qualidade.DTOs;

public record CreateNCDto(
    Guid ObraId, string Numero, string Titulo, string Descricao,
    GravidadeNCEnum Gravidade, string? LocalizacaoObra,
    DateTime? DataPrazoConclusao, Guid? ResponsavelId, Guid? InspecaoId = null);
