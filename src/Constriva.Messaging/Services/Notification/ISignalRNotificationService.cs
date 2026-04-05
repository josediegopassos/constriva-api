namespace Constriva.Messaging.Services.Notification;

/// <summary>
/// Serviço para enviar notificações em tempo real via SignalR através da Constriva.API.
/// </summary>
public interface ISignalRNotificationService
{
    /// <summary>Notifica que o processamento foi atualizado.</summary>
    Task NotificarProcessamentoAtualizadoAsync(Guid usuarioId, Guid? obraId, Guid empresaId, object dados, CancellationToken ct);

    /// <summary>Notifica que o processamento foi concluído com sucesso.</summary>
    Task NotificarProcessamentoConcluidoAsync(Guid usuarioId, Guid? obraId, Guid empresaId, object dados, CancellationToken ct);

    /// <summary>Notifica que ocorreu um erro no processamento.</summary>
    Task NotificarProcessamentoErroAsync(Guid usuarioId, Guid? obraId, Guid empresaId, object dados, CancellationToken ct);

    /// <summary>Notifica que um item foi atualizado.</summary>
    Task NotificarItemAtualizadoAsync(Guid usuarioId, Guid? obraId, Guid empresaId, object dados, CancellationToken ct);

    /// <summary>Notifica que a consolidação foi concluída.</summary>
    Task NotificarConsolidacaoConcluidaAsync(Guid usuarioId, Guid? obraId, Guid empresaId, object dados, CancellationToken ct);
}
