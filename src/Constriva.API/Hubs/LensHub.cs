using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;

namespace Constriva.API.Hubs;

[Authorize]
public class LensHub : Hub
{
    public override async Task OnConnectedAsync()
    {
        var userId = Context.User?.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        if (userId != null)
            await Groups.AddToGroupAsync(Context.ConnectionId, $"user-{userId}");

        var empresaId = Context.User?.FindFirst("empresaId")?.Value;
        if (empresaId != null)
            await Groups.AddToGroupAsync(Context.ConnectionId, $"empresa-{empresaId}");

        var httpContext = Context.GetHttpContext();
        var obraId = httpContext?.Request.Query["obraId"].FirstOrDefault();
        if (!string.IsNullOrEmpty(obraId))
            await Groups.AddToGroupAsync(Context.ConnectionId, $"obra-{obraId}");

        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        await base.OnDisconnectedAsync(exception);
    }

    public async Task JoinObraGroup(string obraId)
        => await Groups.AddToGroupAsync(Context.ConnectionId, $"obra-{obraId}");

    public async Task LeaveObraGroup(string obraId)
        => await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"obra-{obraId}");
}
