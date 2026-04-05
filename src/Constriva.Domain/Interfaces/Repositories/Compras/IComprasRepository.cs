using Constriva.Domain.Entities.Compras;
using Constriva.Domain.Enums;

namespace Constriva.Domain.Interfaces.Repositories;

public interface IComprasRepository
{
    Task<(IEnumerable<PedidoCompra> Items, int Total)> GetPedidosPagedAsync(
        Guid empresaId, Guid? obraId, StatusPedidoCompraEnum? status, int page, int pageSize, CancellationToken ct = default);
    Task<PedidoCompra?> GetPedidoByIdAsync(Guid id, Guid empresaId, CancellationToken ct = default);
    Task AddPedidoAsync(PedidoCompra pedido, CancellationToken ct = default);
    Task UpdatePedidoAsync(PedidoCompra pedido, CancellationToken ct = default);
    Task ReplaceItensPedidoAsync(Guid pedidoId, Guid empresaId, List<ItemPedidoCompra> novosItens, CancellationToken ct = default);
    Task RemoveItensPedidoAsync(Guid pedidoId, IEnumerable<ItemPedidoCompra> trackedItens, CancellationToken ct = default);
    Task<IEnumerable<Cotacao>> GetCotacoesAsync(Guid empresaId, Guid? obraId, CancellationToken ct = default);
    Task<Cotacao?> GetCotacaoByIdAsync(Guid id, Guid empresaId, CancellationToken ct = default);
    Task AddCotacaoAsync(Cotacao cotacao, CancellationToken ct = default);
    Task RemoveItensCotacaoAsync(Guid cotacaoId, CancellationToken ct = default);
    Task ReplaceItensCotacaoAsync(Guid cotacaoId, List<ItemCotacao> novosItens, CancellationToken ct = default);

    // Fornecedores convidados
    Task<IEnumerable<FornecedorCotacao>> GetFornecedoresCotacaoAsync(Guid cotacaoId, Guid empresaId, CancellationToken ct = default);
    Task<FornecedorCotacao?> GetFornecedorCotacaoAsync(Guid cotacaoId, Guid fornecedorId, Guid empresaId, CancellationToken ct = default);
    Task AddFornecedorCotacaoAsync(FornecedorCotacao fc, CancellationToken ct = default);

    // Propostas
    Task<IEnumerable<PropostaCotacao>> GetPropostasCotacaoAsync(Guid cotacaoId, Guid empresaId, CancellationToken ct = default);
    Task<PropostaCotacao?> GetPropostaByIdAsync(Guid propostaId, Guid empresaId, CancellationToken ct = default);
    Task AddPropostaAsync(PropostaCotacao proposta, CancellationToken ct = default);
    Task ReplaceItensPropostaAsync(Guid propostaId, List<ItemPropostaCotacao> novosItens, CancellationToken ct = default);
}
