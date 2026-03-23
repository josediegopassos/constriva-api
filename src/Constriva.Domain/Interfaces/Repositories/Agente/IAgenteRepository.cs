using Constriva.Domain.Entities.Agente;
using Constriva.Domain.Entities.Common;

namespace Constriva.Domain.Interfaces.Repositories;

public interface IAgenteRepository
{
    // Tiers
    Task<IEnumerable<AgenteTier>> GetTiersAsync(CancellationToken ct = default);
    Task<AgenteTier?> GetTierByIdAsync(Guid id, CancellationToken ct = default);

    // Config empresa
    Task<AgenteEmpresaConfig?> GetEmpresaConfigAsync(Guid empresaId, CancellationToken ct = default);
    Task AddEmpresaConfigAsync(AgenteEmpresaConfig config, CancellationToken ct = default);

    // Sessões
    Task<AgenteSessao?> GetSessaoByIdAsync(Guid id, Guid empresaId, CancellationToken ct = default);
    Task<AgenteSessao?> GetSessaoComHistoricoAsync(Guid id, Guid empresaId, CancellationToken ct = default);
    Task<IEnumerable<AgenteSessao>> GetSessoesByUsuarioAsync(Guid empresaId, Guid usuarioId, CancellationToken ct = default);
    Task AddSessaoAsync(AgenteSessao sessao, CancellationToken ct = default);
    Task AddHistoricoAsync(AgenteHistorico historico, CancellationToken ct = default);
    Task<IEnumerable<AgenteHistorico>> GetUltimasMensagensAsync(Guid sessaoId, int quantidade, CancellationToken ct = default);

    // Consumo mensal
    Task<AgenteConsumoMensal?> GetConsumoMensalAsync(Guid empresaId, int ano, int mes, CancellationToken ct = default);
    Task AddConsumoMensalAsync(AgenteConsumoMensal consumo, CancellationToken ct = default);

    // Consumo diário
    Task<AgenteConsumoDiario?> GetConsumoDiarioAsync(Guid empresaId, DateTime data, CancellationToken ct = default);
    Task AddConsumoDiarioAsync(AgenteConsumoDiario consumo, CancellationToken ct = default);
    Task<IEnumerable<AgenteConsumoDiario>> GetConsumoDiarioUltimos30DiasAsync(Guid empresaId, CancellationToken ct = default);

    // Consumo por usuário
    Task<AgenteConsumoUsuario?> GetConsumoUsuarioAsync(Guid empresaId, Guid usuarioId, int ano, int mes, CancellationToken ct = default);
    Task AddConsumoUsuarioAsync(AgenteConsumoUsuario consumo, CancellationToken ct = default);
    Task<IEnumerable<AgenteConsumoUsuario>> GetTopUsuariosAsync(Guid empresaId, int ano, int mes, int top, CancellationToken ct = default);

    // Cotas avulsas
    Task<IEnumerable<AgenteCotaAvulsa>> GetCotasAvulsasAtivasAsync(Guid empresaId, CancellationToken ct = default);
    Task AddCotaAvulsaAsync(AgenteCotaAvulsa cota, CancellationToken ct = default);

    // Notificações
    Task<IEnumerable<Notificacao>> GetNotificacoesAsync(Guid empresaId, bool? lida, CancellationToken ct = default);
    Task<Notificacao?> GetNotificacaoByIdAsync(Guid id, Guid empresaId, CancellationToken ct = default);
    Task AddNotificacaoAsync(Notificacao notificacao, CancellationToken ct = default);

    // Admin
    Task<IEnumerable<AgenteEmpresaConfig>> GetTodasEmpresasAtivasAsync(CancellationToken ct = default);
    Task<IEnumerable<AgenteConsumoMensal>> GetRelatorioMensalAsync(int ano, int mes, CancellationToken ct = default);
}
