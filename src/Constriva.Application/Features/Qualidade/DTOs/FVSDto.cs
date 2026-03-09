namespace Constriva.Application.Features.Qualidade.DTOs;

public record FVSDto(
    Guid Id, Guid ObraId, string Numero, string Servico,
    bool Aprovado, DateTime DataVerificacao, Guid? ResponsavelId);

public record FvsDto(Guid Id, string Servico, Guid? ResponsavelId, DateTime DataVerificacao, bool Aprovado, string? Observacoes, Guid? ObraId);
