using MediatR;
using Constriva.Application.Common.Behaviors;
using Constriva.Domain.Entities.Compras;
using Constriva.Domain.Enums;
using Constriva.Domain.Interfaces.Repositories;
using Constriva.Application.Features.Compras.DTOs;

namespace Constriva.Application.Features.Compras.Commands;

public record UpdatePedidoCompraCommand(Guid Id, Guid EmpresaId, UpdatePedidoDto Dto)
    : IRequest<PedidoCompraDto>, ITenantRequest;

public class UpdatePedidoCompraHandler : IRequestHandler<UpdatePedidoCompraCommand, PedidoCompraDto>
{
    private readonly IComprasRepository _repo;
    private readonly IUnitOfWork _uow;

    public UpdatePedidoCompraHandler(IComprasRepository repo, IUnitOfWork uow)
    {
        _repo = repo;
        _uow = uow;
    }

    public async Task<PedidoCompraDto> Handle(UpdatePedidoCompraCommand request, CancellationToken ct)
    {
        var pedido = await _repo.GetPedidoByIdAsync(request.Id, request.EmpresaId, ct)
            ?? throw new KeyNotFoundException($"Pedido {request.Id} não encontrado.");

        if (pedido.Status != StatusPedidoCompraEnum.Rascunho)
            throw new InvalidOperationException(
                $"Pedido no status '{pedido.Status}' não pode ser editado. Apenas rascunhos podem ser alterados.");

        var dto = request.Dto;

        if (dto.ObraId.HasValue) pedido.ObraId = dto.ObraId.Value;
        if (dto.FornecedorId.HasValue) pedido.FornecedorId = dto.FornecedorId.Value;
        if (dto.AlmoxarifadoId.HasValue) pedido.AlmoxarifadoId = dto.AlmoxarifadoId;
        if (dto.DataEntregaPrevista.HasValue) pedido.DataEntregaPrevista = dto.DataEntregaPrevista;
        if (dto.FormaPagamento.HasValue) pedido.FormaPagamento = dto.FormaPagamento;
        if (dto.CondicoesPagamento is not null) pedido.CondicoesPagamento = dto.CondicoesPagamento;
        if (dto.LocalEntrega is not null) pedido.LocalEntrega = dto.LocalEntrega;
        if (dto.ValorFrete.HasValue) pedido.ValorFrete = dto.ValorFrete.Value;
        if (dto.ValorDesconto.HasValue) pedido.ValorDesconto = dto.ValorDesconto.Value;
        if (dto.Observacoes is not null) pedido.Observacoes = dto.Observacoes;

        // Atualizar itens se enviados
        if (dto.Itens is not null)
        {
            await _repo.ReplaceItensPedidoAsync(pedido.Id, request.EmpresaId, dto.Itens.Select(i => new ItemPedidoCompra
            {
                EmpresaId = request.EmpresaId,
                PedidoId = pedido.Id,
                MaterialId = i.MaterialId,
                Descricao = i.Descricao,
                UnidadeMedida = i.UnidadeMedida,
                QuantidadePedida = i.Quantidade,
                PrecoUnitario = i.PrecoUnitario
            }).ToList(), ct);
        }

        // Recalcular valor total via SQL para evitar problemas com change tracker
        await _repo.UpdatePedidoAsync(pedido, ct);

        // Recarregar para retornar dados atualizados
        var atualizado = await _repo.GetPedidoByIdAsync(request.Id, request.EmpresaId, ct);
        return PedidoCompraMapper.ToDto(atualizado!);
    }
}
