namespace Constriva.Application.Features.SST.DTOs;

public record DDSDto(
    Guid Id, Guid ObraId, string Numero, string Tema, string? Conteudo,
    string? Ministrador, int NumeroParticipantes, DateTime DataRealizacao, DateTime CreatedAt);

public record DdsDto(Guid Id, string Tema, string? Local, DateTime DataRealizacao, int NumeroParticipantes, DateTime CreatedAt);
