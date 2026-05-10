namespace Constriva.Messaging.Services.Notification;

public interface ISignalRNotificationService
{
    Task NotificarProcessamentoAtualizadoAsync(Guid usuarioId, Guid? obraId, Guid empresaId, object dados, CancellationToken ct);

    Task NotificarProcessamentoConcluidoAsync(Guid usuarioId, Guid? obraId, Guid empresaId, object dados, CancellationToken ct);

    Task NotificarProcessamentoErroAsync(Guid usuarioId, Guid? obraId, Guid empresaId, object dados, CancellationToken ct);

    Task NotificarItemAtualizadoAsync(Guid usuarioId, Guid? obraId, Guid empresaId, object dados, CancellationToken ct);

    Task NotificarConsolidacaoConcluidaAsync(Guid usuarioId, Guid? obraId, Guid empresaId, object dados, CancellationToken ct);

    Task NotificarWhatsAppAtualizacaoAsync(Guid cotacaoId, Guid empresaId, object dados, CancellationToken ct);
}
