namespace Constriva.Application.Features.Agente.DTOs;

public record NotificacaoDto(Guid Id, string ModuloOrigem, string Tipo, string Mensagem, bool Lida, DateTime? Prazo, DateTime CreatedAt);
