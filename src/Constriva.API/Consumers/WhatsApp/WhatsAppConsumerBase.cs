using Microsoft.AspNetCore.SignalR;
using Constriva.API.Hubs;

namespace Constriva.API.Consumers.WhatsApp;

public abstract class WhatsAppConsumerBase
{
    protected readonly IHubContext<CotacaoWhatsAppHub> Hub;

    protected WhatsAppConsumerBase(IHubContext<CotacaoWhatsAppHub> hub) => Hub = hub;

    protected async Task NotificarGrupoAsync(Guid cotacaoId, Guid empresaId, object payload, CancellationToken ct)
    {
        await Hub.Clients.Group($"cotacao-{cotacaoId}").SendAsync("WhatsAppStatusAtualizado", payload, ct);
        if (empresaId != Guid.Empty)
            await Hub.Clients.Group($"empresa-{empresaId}").SendAsync("WhatsAppStatusAtualizado", payload, ct);
    }
}
