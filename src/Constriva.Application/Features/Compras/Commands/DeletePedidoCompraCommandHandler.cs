using FluentValidation;
using MediatR;
using Constriva.Application.Common.Behaviors;
using Constriva.Application.Common.Interfaces;
using Constriva.Domain.Entities.Compras;
using Constriva.Domain.Enums;
using Constriva.Domain.Interfaces.Repositories;
using Constriva.Application.Features.Compras.DTOs;

namespace Constriva.Application.Features.Compras.Commands;

public record DeletePedidoCompraCommand(Guid Id, Guid EmpresaId) : IRequest<Unit>, ITenantRequest
    { public Guid TenantId => EmpresaId; }

public class DeletePedidoCompraHandler : IRequestHandler<DeletePedidoCompraCommand, Unit>
{
    private readonly IComprasRepository _repo;
    private readonly IUnitOfWork _uow;

    public DeletePedidoCompraHandler(IComprasRepository repo, IUnitOfWork uow)
    {
        _repo = repo;
        _uow = uow;
    }

    public async Task<Unit> Handle(DeletePedidoCompraCommand request, CancellationToken cancellationToken)
    {
        var pedido = await _repo.GetPedidoByIdAsync(request.Id, request.EmpresaId, cancellationToken)
            ?? throw new KeyNotFoundException($"Pedido {request.Id} não encontrado.");

        pedido.IsDeleted = true;
        await _uow.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}
