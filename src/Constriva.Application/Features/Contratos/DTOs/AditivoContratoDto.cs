namespace Constriva.Application.Features.Contratos.DTOs;

public record AditivoContratoDto(
    Guid Id, Guid ContratoId, string Numero, string Tipo,
    string Justificativa, DateTime DataAssinatura, decimal ValorAditivo,
    int? ProrrogacaoDias, DateTime? NovaDataVigencia, string? ArquivoUrl,
    DateTime CreatedAt);
