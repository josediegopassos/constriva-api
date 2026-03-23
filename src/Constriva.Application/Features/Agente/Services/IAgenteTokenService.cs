using Constriva.Application.Features.Agente.DTOs;

namespace Constriva.Application.Features.Agente.Services;

public interface IAgenteTokenService
{
    Task ValidarCotaAsync(Guid empresaId, CancellationToken ct);
    Task RegistrarConsumoAsync(Guid empresaId, Guid usuarioId, int tokensInput, int tokensOutput, CancellationToken ct);
    Task<DashboardConsumoDto> ObterDashboardConsumoAsync(Guid empresaId, CancellationToken ct);
    Task<IEnumerable<AdminRelatorioItemDto>> ObterRelatorioMensalAsync(int ano, int mes, CancellationToken ct);
    Task AdicionarCotaAvulsaAsync(Guid empresaId, long tokens, string motivo, DateTime? expiracao, Guid adminUserId, CancellationToken ct);
}
