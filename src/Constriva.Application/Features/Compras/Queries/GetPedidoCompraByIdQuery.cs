using MediatR;
using Constriva.Application.Common.Behaviors;
using Constriva.Domain.Interfaces.Repositories;
using Constriva.Application.Features.Compras.DTOs;

namespace Constriva.Application.Features.Compras;

public record GetPedidoCompraByIdQuery(Guid Id, Guid EmpresaId)
    : IRequest<PedidoCompraDto?>, ITenantRequest;

public class GetPedidoCompraByIdHandler : IRequestHandler<GetPedidoCompraByIdQuery, PedidoCompraDto?>
{
    private readonly IComprasRepository _repo;
    public GetPedidoCompraByIdHandler(IComprasRepository repo) => _repo = repo;

    public async Task<PedidoCompraDto?> Handle(GetPedidoCompraByIdQuery request, CancellationToken ct)
    {
        var p = await _repo.GetPedidoByIdAsync(request.Id, request.EmpresaId, ct);
        if (p is null) return null;

        return PedidoCompraMapper.ToDto(p);
    }
}
