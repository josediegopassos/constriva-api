using FluentValidation;
using MediatR;
using Constriva.Application.Common.Behaviors;
using Constriva.Application.Common.Interfaces;
using Constriva.Domain.Entities.Compras;
using Constriva.Domain.Enums;
using Constriva.Domain.Interfaces.Repositories;
using Constriva.Application.Features.Compras.DTOs;

namespace Constriva.Application.Features.Compras.Commands;

public record UpdateStatusPedidoCommand(Guid Id, Guid EmpresaId, StatusPedidoCompraEnum Status)
    : IRequest<Unit>, ITenantRequest;

public class UpdateStatusPedidoHandler : IRequestHandler<UpdateStatusPedidoCommand, Unit>
{
    private readonly IComprasRepository _repo;
    private readonly IUnitOfWork _uow;

    public UpdateStatusPedidoHandler(IComprasRepository repo, IUnitOfWork uow)
    {
        _repo = repo;
        _uow = uow;
    }

    public async Task<Unit> Handle(UpdateStatusPedidoCommand request, CancellationToken cancellationToken)
    {
        var pedido = await _repo.GetPedidoByIdAsync(request.Id, request.EmpresaId, cancellationToken)
            ?? throw new KeyNotFoundException($"Pedido {request.Id} não encontrado.");

        pedido.Status = request.Status;
        await _uow.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}
