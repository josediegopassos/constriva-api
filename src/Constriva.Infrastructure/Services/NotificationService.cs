using Constriva.Application.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Constriva.Infrastructure.Services
{
    public class NotificationService : INotificationService
    {
        public Task SendPushAsync(Guid usuarioId, string title, string body, object? data = null, CancellationToken ct = default)
        {
            // Integrate with Firebase FCM or OneSignal in production
            return Task.CompletedTask;
        }

        public Task SendToEmpresaAsync(Guid empresaId, string title, string body, object? data = null, CancellationToken ct = default)
            => Task.CompletedTask;
    }
}
