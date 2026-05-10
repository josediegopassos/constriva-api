namespace Constriva.Application.Features.Agente.DTOs;

public record MensagemDto(string Role, string Conteudo, int TokensInput, int TokensOutput, DateTime CreatedAt);
