namespace Constriva.Application.Features.SST.DTOs;

public record UpdateDDSDto(string Tema, string? Local, DateTime DataRealizacao, int QuantidadeParticipantes, string? Observacoes);
