using Constriva.Domain.Entities.Compras;
using Constriva.Domain.Entities.WhatsApp;

namespace Constriva.Domain.Interfaces.Repositories;

public interface IWhatsAppCotacaoRepository
{
    Task<CotacaoWhatsApp?> GetCotacaoWhatsAppByCotacaoIdAsync(Guid cotacaoId, Guid empresaId, CancellationToken ct = default);
    Task<CotacaoWhatsApp?> GetCotacaoWhatsAppAtivaAsync(Guid empresaId, CancellationToken ct = default);
    Task<bool> ExisteCotacaoWhatsAppAsync(Guid cotacaoId, Guid empresaId, CancellationToken ct = default);
    Task AddCotacaoWhatsAppAsync(CotacaoWhatsApp entity, CancellationToken ct = default);
    Task<CotacaoWhatsApp?> GetCotacaoWhatsAppComDetalhesAsync(Guid cotacaoId, Guid empresaId, CancellationToken ct = default);

    Task<MensagemWhatsApp?> GetMensagemPorTelefoneAsync(Guid cotacaoWhatsAppId, string telefone, Guid empresaId, CancellationToken ct = default);
    Task AddMensagensAsync(IEnumerable<MensagemWhatsApp> mensagens, CancellationToken ct = default);

    Task<RespostaFornecedorWhatsApp?> GetRespostaByWaMessageIdAsync(string waMessageId, Guid empresaId, CancellationToken ct = default);
    Task<RespostaFornecedorWhatsApp?> GetRespostaByIdAsync(Guid id, Guid empresaId, CancellationToken ct = default);
    Task AddRespostaAsync(RespostaFornecedorWhatsApp entity, CancellationToken ct = default);

    Task<Cotacao?> GetCotacaoComFornecedoresEItensAsync(Guid cotacaoId, Guid empresaId, CancellationToken ct = default);

    Task<PropostaCotacao?> GetPropostaComRelacionamentosAsync(Guid propostaId, Guid cotacaoId, Guid empresaId, CancellationToken ct = default);
    Task<IReadOnlyList<PropostaCotacao>> GetPropostasCotacaoComItensAsync(Guid cotacaoId, Guid empresaId, CancellationToken ct = default);
    Task<bool> ExistePropostaVencedoraAsync(Guid cotacaoId, Guid empresaId, CancellationToken ct = default);

    Task RecalcularMenorPrecoAsync(Guid cotacaoId, Guid empresaId, CancellationToken ct = default);
}
