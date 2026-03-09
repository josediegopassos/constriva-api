using Constriva.Domain.Entities.Compras;
using Constriva.Domain.Enums;

namespace Constriva.Domain.Interfaces.Repositories;

public interface IComprasRepository
{
    Task<(IEnumerable<PedidoCompra> Items, int Total)> GetPedidosPagedAsync(
        Guid empresaId, Guid? obraId, StatusPedidoCompraEnum? status, int page, int pageSize, CancellationToken ct = default);
    Task<PedidoCompra?> GetPedidoByIdAsync(Guid id, Guid empresaId, CancellationToken ct = default);
    Task AddPedidoAsync(PedidoCompra pedido, CancellationToken ct = default);
    Task<IEnumerable<Cotacao>> GetCotacoesAsync(Guid empresaId, Guid? obraId, CancellationToken ct = default);
    Task AddCotacaoAsync(Cotacao cotacao, CancellationToken ct = default);
}
