namespace Constriva.Application.Features.Agente.DTOs;

public record DashboardConsumoDto(
    ConsumoResumoDto ConsumoMesAtual,
    TierDto TierContratado,
    long CotaAvulsaDisponivel,
    IEnumerable<ConsumoDiarioDto> HistoricoDiario,
    IEnumerable<ConsumoUsuarioDto> TopUsuarios,
    int DiasRestantesNoMes);
