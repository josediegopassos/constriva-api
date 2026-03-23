using Microsoft.EntityFrameworkCore;
using Constriva.Domain.Entities.Agente;
using Constriva.Domain.Entities.Common;
using Constriva.Domain.Interfaces.Repositories;

namespace Constriva.Infrastructure.Persistence.Repositories;

public class AgenteRepository : IAgenteRepository
{
    private readonly AppDbContext _ctx;
    public AgenteRepository(AppDbContext ctx) => _ctx = ctx;

    // ─── Tiers ──────────────────────────────────────────────────────────────────
    public async Task<IEnumerable<AgenteTier>> GetTiersAsync(CancellationToken ct = default)
        => await _ctx.AgenteTiers.Where(t => !t.IsDeleted).OrderBy(t => t.TokensMensais).ToListAsync(ct);

    public async Task<AgenteTier?> GetTierByIdAsync(Guid id, CancellationToken ct = default)
        => await _ctx.AgenteTiers.FirstOrDefaultAsync(t => t.Id == id && !t.IsDeleted, ct);

    // ─── Config empresa ─────────────────────────────────────────────────────────
    public async Task<AgenteEmpresaConfig?> GetEmpresaConfigAsync(Guid empresaId, CancellationToken ct = default)
        => await _ctx.AgenteEmpresaConfigs
            .Include(c => c.Tier)
            .FirstOrDefaultAsync(c => c.EmpresaId == empresaId && !c.IsDeleted, ct);

    public async Task AddEmpresaConfigAsync(AgenteEmpresaConfig config, CancellationToken ct = default)
        => await _ctx.AgenteEmpresaConfigs.AddAsync(config, ct);

    // ─── Sessões ────────────────────────────────────────────────────────────────
    public async Task<AgenteSessao?> GetSessaoByIdAsync(Guid id, Guid empresaId, CancellationToken ct = default)
        => await _ctx.AgenteSessoes
            .FirstOrDefaultAsync(s => s.Id == id && s.EmpresaId == empresaId && !s.IsDeleted, ct);

    public async Task<AgenteSessao?> GetSessaoComHistoricoAsync(Guid id, Guid empresaId, CancellationToken ct = default)
        => await _ctx.AgenteSessoes
            .Include(s => s.Mensagens)
            .FirstOrDefaultAsync(s => s.Id == id && s.EmpresaId == empresaId && !s.IsDeleted, ct);

    public async Task<IEnumerable<AgenteSessao>> GetSessoesByUsuarioAsync(Guid empresaId, Guid usuarioId, CancellationToken ct = default)
        => await _ctx.AgenteSessoes
            .Where(s => s.EmpresaId == empresaId && s.UsuarioId == usuarioId && !s.IsDeleted)
            .OrderByDescending(s => s.AtualizadaEm)
            .ToListAsync(ct);

    public async Task AddSessaoAsync(AgenteSessao sessao, CancellationToken ct = default)
        => await _ctx.AgenteSessoes.AddAsync(sessao, ct);

    public async Task AddHistoricoAsync(AgenteHistorico historico, CancellationToken ct = default)
        => await _ctx.AgenteHistoricos.AddAsync(historico, ct);

    public async Task<IEnumerable<AgenteHistorico>> GetUltimasMensagensAsync(Guid sessaoId, int quantidade, CancellationToken ct = default)
    {
        var mensagens = await _ctx.AgenteHistoricos
            .Where(m => m.SessaoId == sessaoId && !m.IsDeleted)
            .OrderByDescending(m => m.CreatedAt)
            .Take(quantidade)
            .ToListAsync(ct);

        mensagens.Reverse();
        return mensagens;
    }

    // ─── Consumo mensal ─────────────────────────────────────────────────────────
    public async Task<AgenteConsumoMensal?> GetConsumoMensalAsync(Guid empresaId, int ano, int mes, CancellationToken ct = default)
        => await _ctx.AgenteConsumoMensal
            .FirstOrDefaultAsync(c => c.EmpresaId == empresaId && c.Ano == ano && c.Mes == mes && !c.IsDeleted, ct);

    public async Task AddConsumoMensalAsync(AgenteConsumoMensal consumo, CancellationToken ct = default)
        => await _ctx.AgenteConsumoMensal.AddAsync(consumo, ct);

    // ─── Consumo diário ─────────────────────────────────────────────────────────
    public async Task<AgenteConsumoDiario?> GetConsumoDiarioAsync(Guid empresaId, DateTime data, CancellationToken ct = default)
        => await _ctx.AgenteConsumoDiario
            .FirstOrDefaultAsync(c => c.EmpresaId == empresaId && c.Data == data && !c.IsDeleted, ct);

    public async Task AddConsumoDiarioAsync(AgenteConsumoDiario consumo, CancellationToken ct = default)
        => await _ctx.AgenteConsumoDiario.AddAsync(consumo, ct);

    public async Task<IEnumerable<AgenteConsumoDiario>> GetConsumoDiarioUltimos30DiasAsync(Guid empresaId, CancellationToken ct = default)
    {
        var dataLimite = DateTime.UtcNow.AddDays(-30);
        return await _ctx.AgenteConsumoDiario
            .Where(c => c.EmpresaId == empresaId && c.Data >= dataLimite && !c.IsDeleted)
            .OrderBy(c => c.Data)
            .ToListAsync(ct);
    }

    // ─── Consumo por usuário ────────────────────────────────────────────────────
    public async Task<AgenteConsumoUsuario?> GetConsumoUsuarioAsync(Guid empresaId, Guid usuarioId, int ano, int mes, CancellationToken ct = default)
        => await _ctx.AgenteConsumoUsuario
            .FirstOrDefaultAsync(c => c.EmpresaId == empresaId && c.UsuarioId == usuarioId && c.Ano == ano && c.Mes == mes && !c.IsDeleted, ct);

    public async Task AddConsumoUsuarioAsync(AgenteConsumoUsuario consumo, CancellationToken ct = default)
        => await _ctx.AgenteConsumoUsuario.AddAsync(consumo, ct);

    public async Task<IEnumerable<AgenteConsumoUsuario>> GetTopUsuariosAsync(Guid empresaId, int ano, int mes, int top, CancellationToken ct = default)
        => await _ctx.AgenteConsumoUsuario
            .Where(c => c.EmpresaId == empresaId && c.Ano == ano && c.Mes == mes && !c.IsDeleted)
            .OrderByDescending(u => u.TokensUtilizados)
            .Take(top)
            .ToListAsync(ct);

    // ─── Cotas avulsas ──────────────────────────────────────────────────────────
    public async Task<IEnumerable<AgenteCotaAvulsa>> GetCotasAvulsasAtivasAsync(Guid empresaId, CancellationToken ct = default)
        => await _ctx.AgenteCotasAvulsas
            .Where(c => c.EmpresaId == empresaId
                && c.TokensUtilizados < c.TokensConcedidos
                && (c.ExpiraEm == null || c.ExpiraEm > DateTime.UtcNow)
                && !c.IsDeleted)
            .ToListAsync(ct);

    public async Task AddCotaAvulsaAsync(AgenteCotaAvulsa cota, CancellationToken ct = default)
        => await _ctx.AgenteCotasAvulsas.AddAsync(cota, ct);

    // ─── Notificações ───────────────────────────────────────────────────────────
    public async Task<IEnumerable<Notificacao>> GetNotificacoesAsync(Guid empresaId, bool? lida, CancellationToken ct = default)
    {
        var q = _ctx.Notificacoes.Where(n => n.EmpresaId == empresaId && !n.IsDeleted);
        if (lida.HasValue) q = q.Where(n => n.Lida == lida.Value);
        return await q.OrderByDescending(n => n.CreatedAt).ToListAsync(ct);
    }

    public async Task<Notificacao?> GetNotificacaoByIdAsync(Guid id, Guid empresaId, CancellationToken ct = default)
        => await _ctx.Notificacoes
            .FirstOrDefaultAsync(n => n.Id == id && n.EmpresaId == empresaId && !n.IsDeleted, ct);

    public async Task AddNotificacaoAsync(Notificacao notificacao, CancellationToken ct = default)
        => await _ctx.Notificacoes.AddAsync(notificacao, ct);

    // ─── Admin ──────────────────────────────────────────────────────────────────
    public async Task<IEnumerable<AgenteEmpresaConfig>> GetTodasEmpresasAtivasAsync(CancellationToken ct = default)
        => await _ctx.AgenteEmpresaConfigs
            .Include(c => c.Tier)
            .Where(c => c.Ativo == true && !c.IsDeleted)
            .ToListAsync(ct);

    public async Task<IEnumerable<AgenteConsumoMensal>> GetRelatorioMensalAsync(int ano, int mes, CancellationToken ct = default)
        => await _ctx.AgenteConsumoMensal
            .Where(c => c.Ano == ano && c.Mes == mes && !c.IsDeleted)
            .ToListAsync(ct);
}
