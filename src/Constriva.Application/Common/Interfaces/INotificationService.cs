namespace Constriva.Application.Common.Interfaces;

public interface INotificationService
{
    Task SendPushAsync(Guid usuarioId, string title, string body, object? data = null, CancellationToken ct = default);
    Task SendToEmpresaAsync(Guid empresaId, string title, string body, object? data = null, CancellationToken ct = default);
}
