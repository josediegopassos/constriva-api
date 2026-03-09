namespace Constriva.Application.Features.SST.DTOs;

public record CreateDDSDto(
    Guid ObraId, string Numero, string Tema, string? Conteudo,
    string? Ministrador, int NumeroParticipantes, DateTime DataRealizacao);
