namespace Constriva.Application.Features.Agente.DTOs;

public record SessaoResumoDto(Guid Id, DateTime AtualizadaEm, bool Ativa, int TotalMensagens);
