namespace Constriva.Application.Features.Agente.Services;

public interface IConstrivaToolsService
{
    Task<string> ExecuteToolAsync(string toolName, string argsJson, Guid empresaId, Guid usuarioId, CancellationToken ct);
    IReadOnlyList<object> GetToolDefinitions();
}
