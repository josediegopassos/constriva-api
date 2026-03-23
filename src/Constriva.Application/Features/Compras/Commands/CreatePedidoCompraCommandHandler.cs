using FluentValidation;
using MediatR;
using Constriva.Application.Common.Behaviors;
using Constriva.Application.Common.Interfaces;
using Constriva.Domain.Entities.Compras;
using Constriva.Domain.Entities.Fornecedores;
using Constriva.Domain.Enums;
using Constriva.Domain.Interfaces.Repositories;
using Constriva.Application.Features.Compras.DTOs;

namespace Constriva.Application.Features.Compras.Commands;

public record CreatePedidoCompraCommand(Guid EmpresaId, CreatePedidoDto Dto, Guid UsuarioId = default)
    : IRequest<PedidoCompraDto>, ITenantRequest;

public class CreatePedidoCompraHandler : IRequestHandler<CreatePedidoCompraCommand, PedidoCompraDto>
{
    private readonly IComprasRepository _repo;
    private readonly IObraRepository _obraRepo;
    private readonly IFornecedorRepository _fornecedorRepo;
    private readonly IMaterialRepository _materialRepo;
    private readonly IUnitOfWork _uow;

    public CreatePedidoCompraHandler(
        IComprasRepository repo, IObraRepository obraRepo,
        IFornecedorRepository fornecedorRepo, IMaterialRepository materialRepo, IUnitOfWork uow)
    {
        _repo = repo;
        _obraRepo = obraRepo;
        _fornecedorRepo = fornecedorRepo;
        _materialRepo = materialRepo;
        _uow = uow;
    }

    public async Task<PedidoCompraDto> Handle(CreatePedidoCompraCommand request, CancellationToken cancellationToken)
    {
        var dto = request.Dto;

        if (dto.ObraId is null)
            throw new InvalidOperationException("ObraId é obrigatório para criar um pedido de compra.");
        if (dto.FornecedorId is null)
            throw new InvalidOperationException("FornecedorId é obrigatório para criar um pedido de compra.");

        _ = await _obraRepo.GetByIdAndEmpresaAsync(dto.ObraId.Value, request.EmpresaId, cancellationToken)
            ?? throw new KeyNotFoundException($"Obra {dto.ObraId} não encontrada.");

        _ = await _fornecedorRepo.GetByIdAndEmpresaAsync(dto.FornecedorId.Value, request.EmpresaId, cancellationToken)
            ?? throw new KeyNotFoundException($"Fornecedor {dto.FornecedorId} não encontrado.");

        var numero = $"PC-{DateTime.UtcNow:yyyyMMddHHmmss}";

        var pedido = new PedidoCompra
        {
            EmpresaId = request.EmpresaId,
            ObraId = dto.ObraId.Value,
            FornecedorId = dto.FornecedorId.Value,
            Numero = numero,
            Status = StatusPedidoCompraEnum.Rascunho,
            DataPedido = DateTime.UtcNow,
            Observacoes = dto.Observacoes,
            CriadoPor = request.UsuarioId
        };

        foreach (var item in dto.Itens)
        {
            // MaterialId é opcional: itens podem ser criados por descrição livre.
            // Quando informado, valida existência e pertença ao tenant.
            Guid? materialId = null;
            if (item.MaterialId.HasValue)
            {
                _ = await _materialRepo.GetByIdAndEmpresaAsync(item.MaterialId.Value, request.EmpresaId, cancellationToken)
                    ?? throw new KeyNotFoundException($"Material {item.MaterialId} não encontrado.");
                materialId = item.MaterialId.Value;
            }

            pedido.Itens.Add(new ItemPedidoCompra
            {
                EmpresaId = request.EmpresaId,
                MaterialId = materialId,
                Descricao = item.Descricao,
                UnidadeMedida = item.UnidadeMedida,
                QuantidadePedida = item.Quantidade,
                PrecoUnitario = item.PrecoUnitario
            });
        }

        pedido.ValorTotal = pedido.Itens.Sum(i => i.ValorTotal);

        await _repo.AddPedidoAsync(pedido, cancellationToken);
        await _uow.SaveChangesAsync(cancellationToken);

        var itensDto = pedido.Itens.Select(i => new ItemPedidoDto(i.Id, i.Descricao, i.UnidadeMedida, i.QuantidadePedida, i.PrecoUnitario, i.ValorTotal));
        return new PedidoCompraDto(
            pedido.Id, pedido.Numero, pedido.ObraId, null,
            pedido.FornecedorId, null, pedido.Status, pedido.ValorTotal,
            pedido.DataPedido, pedido.Observacoes, itensDto);
    }
}
