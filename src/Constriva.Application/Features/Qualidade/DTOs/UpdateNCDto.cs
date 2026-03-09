using Constriva.Domain.Enums;

namespace Constriva.Application.Features.Qualidade.DTOs;

public record UpdateNCDto(string Descricao, string? Causa, string? AcaoCorretiva, DateTime? DataPrazo, StatusNaoConformidadeEnum Status);
