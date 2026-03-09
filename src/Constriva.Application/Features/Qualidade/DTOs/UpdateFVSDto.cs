namespace Constriva.Application.Features.Qualidade.DTOs;

public record UpdateFVSDto(string Servico, Guid? Responsavel, DateTime DataVerificacao, bool Aprovado, string? Observacoes);
