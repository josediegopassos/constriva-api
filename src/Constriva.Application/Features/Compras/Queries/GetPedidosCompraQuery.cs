using MediatR;
using Constriva.Application.Common.Behaviors;
using Constriva.Domain.Entities.Compras;
using Constriva.Domain.Enums;
using Constriva.Domain.Interfaces.Repositories;
using Constriva.Application.Features.Compras.DTOs;
using Constriva.Application.Common.Interfaces;

namespace Constriva.Application.Features.Compras;

public record GetPedidosCompraQuery(
    Guid EmpresaId, Guid? ObraId = null, StatusPedidoCompraEnum? Status = null,
    int Page = 1, int PageSize = 20)
    : IRequest<PaginatedResult<PedidoCompraDto>>, ITenantRequest;

public class GetPedidosCompraHandler : IRequestHandler<GetPedidosCompraQuery, PaginatedResult<PedidoCompraDto>>
{
    private readonly IComprasRepository _repo;
    public GetPedidosCompraHandler(IComprasRepository repo) => _repo = repo;

    public async Task<PaginatedResult<PedidoCompraDto>> Handle(GetPedidosCompraQuery request, CancellationToken ct)
    {
        var (items, total) = await _repo.GetPedidosPagedAsync(
            request.EmpresaId, request.ObraId, request.Status, request.Page, request.PageSize, ct);

        return new PaginatedResult<PedidoCompraDto>
        {
            Items = items.Select(MapPedido),
            TotalCount = total, Page = request.Page, PageSize = request.PageSize
        };
    }

    private static PedidoCompraDto MapPedido(PedidoCompra p) => PedidoCompraMapper.ToDto(p);
}
