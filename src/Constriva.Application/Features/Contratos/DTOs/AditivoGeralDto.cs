namespace Constriva.Application.Features.Contratos.DTOs;

public record AditivoGeralDto(
    Guid Id, Guid ContratoId, string NumeroContrato, string? NomeFornecedor,
    string Numero, string Tipo, string Justificativa,
    DateTime DataAssinatura, decimal ValorAditivo,
    int? ProrrogacaoDias, DateTime? NovaDataVigencia, string? ArquivoUrl,
    DateTime CreatedAt);
