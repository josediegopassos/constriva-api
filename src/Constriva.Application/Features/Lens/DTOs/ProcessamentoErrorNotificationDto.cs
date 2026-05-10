namespace Constriva.Application.Features.Lens.DTOs;

public record ProcessamentoErrorNotificationDto(
    Guid ProcessamentoId,
    string Status,
    string TipoDocumento,
    string CodigoErro,
    string MensagemErro,
    bool PodeReprocessar,
    string Mensagem);
