using Constriva.Domain.Enums;

namespace Constriva.Application.Features.Lens.DTOs;

public record ProcessamentoUpdatedNotificationDto(
    Guid ProcessamentoId,
    StatusProcessamentoLensEnum Status,
    string StatusDescricao,
    string TipoDocumento,
    DateTime AtualizadoEm,
    string Mensagem);
