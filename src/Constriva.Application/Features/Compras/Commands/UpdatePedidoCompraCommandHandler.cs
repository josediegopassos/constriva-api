using FluentValidation;
using MediatR;
using Constriva.Application.Common.Behaviors;
using Constriva.Application.Common.Interfaces;
using Constriva.Domain.Entities.Compras;
using Constriva.Domain.Enums;
using Constriva.Domain.Interfaces.Repositories;
using Constriva.Application.Features.Compras.DTOs;

namespace Constriva.Application.Features.Compras.Commands;

public record UpdatePedidoCompraCommand(Guid Id, Guid EmpresaId, Guid? FornecedorId,
    DateTime? DataEntregaPrevista, string? Observacoes)
    : IRequest<PedidoCompraDto>, ITenantRequest { public Guid TenantId => EmpresaId; }

public class UpdatePedidoCompraHandler : IRequestHandler<UpdatePedidoCompraCommand, PedidoCompraDto>
{
    private readonly IComprasRepository _repo;
    private readonly IUnitOfWork _uow;

    public UpdatePedidoCompraHandler(IComprasRepository repo, IUnitOfWork uow)
    {
        _repo = repo;
        _uow = uow;
    }

    public async Task<PedidoCompraDto> Handle(UpdatePedidoCompraCommand request, CancellationToken cancellationToken)
    {
        var pedido = await _repo.GetPedidoByIdAsync(request.Id, request.EmpresaId, cancellationToken)
            ?? throw new KeyNotFoundException($"Pedido {request.Id} não encontrado.");

        if (pedido.Status != StatusPedidoCompraEnum.Rascunho)
            throw new InvalidOperationException(
                $"Pedido no status '{pedido.Status}' não pode ser editado. Apenas rascunhos podem ser alterados.");

        if (request.FornecedorId.HasValue) pedido.FornecedorId = request.FornecedorId.Value;
        if (request.DataEntregaPrevista.HasValue) pedido.DataEntregaPrevista = request.DataEntregaPrevista;
        if (request.Observacoes != null) pedido.Observacoes = request.Observacoes;

        await _uow.SaveChangesAsync(cancellationToken);

        var itensDto = pedido.Itens.Select(i => new ItemPedidoDto(i.Id, i.Descricao, i.UnidadeMedida, i.QuantidadePedida, i.PrecoUnitario, i.ValorTotal));
        return new PedidoCompraDto(
            pedido.Id, pedido.Numero, pedido.ObraId == Guid.Empty ? null : pedido.ObraId,
            null, pedido.FornecedorId, null, pedido.Status,
            pedido.ValorTotal, pedido.DataPedido, pedido.Observacoes, itensDto);
    }
}
