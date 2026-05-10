namespace Constriva.Application.Features.Lens.DTOs;

public record ItemUpdatedNotificationDto(
    Guid ProcessamentoId,
    Guid ItemId,
    string Acao,
    Guid UsuarioId,
    string NomeUsuario,
    DateTime AtualizadoEm);
