namespace Constriva.Application.Features.Agente.DTOs;

public record ConsumoUsuarioDto(Guid UsuarioId, string NomeUsuario, long TokensUtilizados, int TotalRequisicoes);
