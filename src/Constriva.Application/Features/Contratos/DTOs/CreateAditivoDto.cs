namespace Constriva.Application.Features.Contratos.DTOs;

public record CreateAditivoDto(
    string Tipo, string Justificativa, DateTime DataAssinatura,
    decimal ValorAditivo, int? ProrrogacaoDias, DateTime? NovaDataVigencia,
    string? ArquivoUrl);
