namespace Constriva.Application.Features.Lens.Interfaces;

public interface ILensLogService
{
    Task<object?> GetLogByProcessamentoIdAsync(Guid processamentoId, Guid empresaId, CancellationToken ct);
}
