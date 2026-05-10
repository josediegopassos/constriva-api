using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace Constriva.API.Hubs;

[Authorize]
public class CotacaoWhatsAppHub : Hub
{
    private readonly ILogger<CotacaoWhatsAppHub> _logger;

    public CotacaoWhatsAppHub(ILogger<CotacaoWhatsAppHub> logger) => _logger = logger;

    public Task EntrarGrupo(string cotacaoId)
    {
        var valor = cotacaoId.Contains(':') ? cotacaoId.Split(':').Last() : cotacaoId;
        if (!Guid.TryParse(valor, out var id))
            throw new HubException($"cotacaoId inválido: '{cotacaoId}'");
        return EntrarGrupoCotacao(id);
    }

    public async Task EntrarGrupoCotacao(Guid cotacaoId)
    {
        var grupo = $"cotacao-{cotacaoId}";
        await Groups.AddToGroupAsync(Context.ConnectionId, grupo);

        var empresaId = ObterEmpresaId();
        if (empresaId.HasValue)
            await Groups.AddToGroupAsync(Context.ConnectionId, $"empresa-{empresaId.Value}");

        _logger.LogInformation("Cliente {Conn} entrou no grupo {Grupo} (EmpresaId: {E})",
            Context.ConnectionId, grupo, empresaId?.ToString() ?? "N/A");

        await Clients.Caller.SendAsync("JoinedGroup", new { cotacaoId, grupo, timestamp = DateTime.UtcNow });
    }

    public async Task SairGrupoCotacao(Guid cotacaoId)
    {
        var grupo = $"cotacao-{cotacaoId}";
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, grupo);
        _logger.LogInformation("Cliente {Conn} saiu do grupo {Grupo}", Context.ConnectionId, grupo);
    }

    public override async Task OnConnectedAsync()
    {
        _logger.LogInformation("Nova conexão SignalR WhatsApp. ConnectionId: {Conn} | UserId: {User}",
            Context.ConnectionId, Context.UserIdentifier);
        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        if (exception != null)
            _logger.LogWarning(exception, "Desconexão com erro. ConnectionId: {Conn}", Context.ConnectionId);
        else
            _logger.LogInformation("Desconexão normal. ConnectionId: {Conn}", Context.ConnectionId);

        await base.OnDisconnectedAsync(exception);
    }

    private Guid? ObterEmpresaId()
    {
        var claim = Context.User?.FindFirst("empresaId");
        if (claim != null && Guid.TryParse(claim.Value, out var empresaId))
            return empresaId;

        _logger.LogWarning("EmpresaId não encontrado no token para ConnectionId: {Conn}. Claims: {Claims}",
            Context.ConnectionId,
            string.Join(", ", Context.User?.Claims.Select(c => $"{c.Type}={c.Value}") ?? []));
        return null;
    }
}
