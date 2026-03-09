using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Text.Json;

namespace Constriva.API.Hubs
{
    [Authorize]
    public class NotificationHub : Hub
    {
        public async Task JoinEmpresaGroup(string empresaId)
            => await Groups.AddToGroupAsync(Context.ConnectionId, $"empresa-{empresaId}");

        public async Task JoinObraGroup(string obraId)
            => await Groups.AddToGroupAsync(Context.ConnectionId, $"obra-{obraId}");

        public async Task LeaveObraGroup(string obraId)
            => await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"obra-{obraId}");

        public override async Task OnConnectedAsync()
        {
            var userId = Context.User?.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (userId != null)
                await Groups.AddToGroupAsync(Context.ConnectionId, $"user-{userId}");
            await base.OnConnectedAsync();
        }
    }
}
