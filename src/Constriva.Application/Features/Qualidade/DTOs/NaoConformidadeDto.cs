using Constriva.Domain.Enums;

namespace Constriva.Application.Features.Qualidade.DTOs;

public record NaoConformidadeDto(
    Guid Id, Guid ObraId, string Numero, string Titulo, string Descricao,
    StatusNaoConformidadeEnum Status, GravidadeNCEnum Gravidade,
    string? LocalizacaoObra, string? CausaRaiz, string? AcaoCorretiva,
    DateTime DataAbertura, DateTime? DataPrazoConclusao, DateTime? DataEncerramento);
