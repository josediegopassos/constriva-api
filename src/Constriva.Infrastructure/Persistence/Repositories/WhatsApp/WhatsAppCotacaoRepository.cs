using Microsoft.EntityFrameworkCore;
using Constriva.Domain.Entities.Compras;
using Constriva.Domain.Entities.WhatsApp;
using Constriva.Domain.Enums;
using Constriva.Domain.Interfaces.Repositories;

namespace Constriva.Infrastructure.Persistence.Repositories.WhatsApp;

public class WhatsAppCotacaoRepository : IWhatsAppCotacaoRepository
{
    private readonly AppDbContext _ctx;

    public WhatsAppCotacaoRepository(AppDbContext ctx) => _ctx = ctx;

    // ── CotacaoWhatsApp ───────────────────────────────────────

    public async Task<CotacaoWhatsApp?> GetCotacaoWhatsAppByCotacaoIdAsync(Guid cotacaoId, Guid empresaId, CancellationToken ct)
        => await _ctx.CotacoesWhatsApp
            .FirstOrDefaultAsync(c => c.CotacaoId == cotacaoId && c.EmpresaId == empresaId && !c.IsDeleted, ct);

    public async Task<CotacaoWhatsApp?> GetCotacaoWhatsAppAtivaAsync(Guid empresaId, CancellationToken ct)
        => await _ctx.CotacoesWhatsApp
            .Where(c => c.EmpresaId == empresaId && !c.IsDeleted && c.EncerradaEm == null)
            .OrderByDescending(c => c.DisparadaEm)
            .FirstOrDefaultAsync(ct);

    public async Task<bool> ExisteCotacaoWhatsAppAsync(Guid cotacaoId, Guid empresaId, CancellationToken ct)
        => await _ctx.CotacoesWhatsApp
            .AnyAsync(c => c.CotacaoId == cotacaoId && c.EmpresaId == empresaId && !c.IsDeleted, ct);

    public async Task AddCotacaoWhatsAppAsync(CotacaoWhatsApp entity, CancellationToken ct)
        => await _ctx.CotacoesWhatsApp.AddAsync(entity, ct);

    public async Task<CotacaoWhatsApp?> GetCotacaoWhatsAppComDetalhesAsync(Guid cotacaoId, Guid empresaId, CancellationToken ct)
        => await _ctx.CotacoesWhatsApp
            .Include(c => c.Mensagens.Where(m => !m.IsDeleted))
            .Include(c => c.Respostas.Where(r => !r.IsDeleted))
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.CotacaoId == cotacaoId && c.EmpresaId == empresaId && !c.IsDeleted, ct);

    // ── MensagemWhatsApp ──────────────────────────────────────

    public async Task<MensagemWhatsApp?> GetMensagemPorTelefoneAsync(Guid cotacaoWhatsAppId, string telefone, Guid empresaId, CancellationToken ct)
        => await _ctx.MensagensWhatsApp
            .Where(m => m.CotacaoWhatsAppId == cotacaoWhatsAppId &&
                        m.TelefoneDestino == telefone &&
                        m.EmpresaId == empresaId && !m.IsDeleted)
            .OrderBy(m => m.CreatedAt)
            .FirstOrDefaultAsync(ct);

    public async Task AddMensagensAsync(IEnumerable<MensagemWhatsApp> mensagens, CancellationToken ct)
        => await _ctx.MensagensWhatsApp.AddRangeAsync(mensagens, ct);

    // ── RespostaFornecedorWhatsApp ────────────────────────────

    public async Task<RespostaFornecedorWhatsApp?> GetRespostaByWaMessageIdAsync(string waMessageId, Guid empresaId, CancellationToken ct)
        => await _ctx.RespostasFornecedorWhatsApp
            .FirstOrDefaultAsync(r => r.WaMessageId == waMessageId && r.EmpresaId == empresaId, ct);

    public async Task<RespostaFornecedorWhatsApp?> GetRespostaByIdAsync(Guid id, Guid empresaId, CancellationToken ct)
        => await _ctx.RespostasFornecedorWhatsApp
            .FirstOrDefaultAsync(r => r.Id == id && r.EmpresaId == empresaId && !r.IsDeleted, ct);

    public async Task AddRespostaAsync(RespostaFornecedorWhatsApp entity, CancellationToken ct)
        => await _ctx.RespostasFornecedorWhatsApp.AddAsync(entity, ct);

    // ── Cotacao com includes ──────────────────────────────────

    public async Task<Cotacao?> GetCotacaoComFornecedoresEItensAsync(Guid cotacaoId, Guid empresaId, CancellationToken ct)
        => await _ctx.Cotacoes
            .Include(c => c.FornecedoresConvidados)
                .ThenInclude(fc => fc.Fornecedor)
            .Include(c => c.Itens)
            .FirstOrDefaultAsync(c => c.Id == cotacaoId && c.EmpresaId == empresaId && !c.IsDeleted, ct);

    // ── PropostaCotacao ───────────────────────────────────────

    public async Task<PropostaCotacao?> GetPropostaComRelacionamentosAsync(Guid propostaId, Guid cotacaoId, Guid empresaId, CancellationToken ct)
        => await _ctx.PropostasCotacao
            .Include(p => p.Itens).ThenInclude(i => i.ItemCotacao)
            .Include(p => p.Fornecedor)
            .Include(p => p.Cotacao)
            .FirstOrDefaultAsync(p =>
                p.Id == propostaId && p.CotacaoId == cotacaoId &&
                p.EmpresaId == empresaId && !p.IsDeleted, ct);

    public async Task<IReadOnlyList<PropostaCotacao>> GetPropostasCotacaoComItensAsync(Guid cotacaoId, Guid empresaId, CancellationToken ct)
        => await _ctx.PropostasCotacao
            .Include(p => p.Itens).ThenInclude(i => i.ItemCotacao)
            .Include(p => p.Fornecedor)
            .AsNoTracking()
            .Where(p => p.CotacaoId == cotacaoId && p.EmpresaId == empresaId && !p.IsDeleted)
            .ToListAsync(ct);

    public async Task<bool> ExistePropostaVencedoraAsync(Guid cotacaoId, Guid empresaId, CancellationToken ct)
        => await _ctx.PropostasCotacao
            .AnyAsync(p => p.CotacaoId == cotacaoId && p.Vencedora &&
                          p.EmpresaId == empresaId && !p.IsDeleted, ct);

    // ── Recalculo MenorPreco ──────────────────────────────────

    public async Task RecalcularMenorPrecoAsync(Guid cotacaoId, Guid empresaId, CancellationToken ct)
    {
        var todosItens = await _ctx.ItensPropostaCotacao
            .Include(i => i.Proposta)
            .Where(i => i.Proposta.CotacaoId == cotacaoId &&
                        i.Proposta.EmpresaId == empresaId &&
                        !i.IsDeleted && !i.Proposta.IsDeleted)
            .ToListAsync(ct);

        foreach (var grupo in todosItens.GroupBy(i => i.ItemCotacaoId))
        {
            var menorPreco = grupo.Min(i => i.PrecoUnitario);
            foreach (var item in grupo)
                item.MenorPreco = item.PrecoUnitario == menorPreco;
        }

        await _ctx.SaveChangesAsync(ct);
    }
}
