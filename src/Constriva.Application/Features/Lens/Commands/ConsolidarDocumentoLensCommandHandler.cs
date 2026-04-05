using MediatR;
using MassTransit;
using Constriva.Application.Common.Behaviors;
using Constriva.Application.Features.Lens.DTOs;
using Constriva.Domain.Entities.Compras;
using Constriva.Domain.Entities.Lens;
using Constriva.Domain.Enums;
using Constriva.Domain.Interfaces.Repositories;
using Constriva.Messaging.Contracts.Lens.Events;

namespace Constriva.Application.Features.Lens.Commands;

public record ConsolidarDocumentoLensCommand(
    Guid ProcessamentoId,
    Guid EmpresaId,
    Guid UsuarioId,
    ConsolidarDocumentoLensDto Dto) : IRequest<Guid>, ITenantRequest;

public class ConsolidarDocumentoLensCommandHandler : IRequestHandler<ConsolidarDocumentoLensCommand, Guid>
{
    private readonly ITenantRepository<DocumentoLens> _docRepo;
    private readonly IRepository<ItemDocumentoLens> _itemRepo;
    private readonly IComprasRepository _comprasRepo;
    private readonly IUnitOfWork _uow;
    private readonly IBus _bus;

    public ConsolidarDocumentoLensCommandHandler(
        ITenantRepository<DocumentoLens> docRepo,
        IRepository<ItemDocumentoLens> itemRepo,
        IComprasRepository comprasRepo,
        IUnitOfWork uow,
        IBus bus)
    {
        _docRepo = docRepo;
        _itemRepo = itemRepo;
        _comprasRepo = comprasRepo;
        _uow = uow;
        _bus = bus;
    }

    public async Task<Guid> Handle(ConsolidarDocumentoLensCommand r, CancellationToken ct)
    {
        var documento = await _docRepo.GetByIdAndEmpresaAsync(r.ProcessamentoId, r.EmpresaId, ct)
            ?? throw new KeyNotFoundException($"Documento Lens {r.ProcessamentoId} não encontrado.");

        if (documento.Status != StatusProcessamentoLensEnum.AguardandoRevisao &&
            documento.Status != StatusProcessamentoLensEnum.EmRevisao)
            throw new InvalidOperationException(
                $"Documento com status '{documento.Status}' não pode ser consolidado. Apenas documentos com status 'AguardandoRevisao' ou 'EmRevisao' são permitidos.");

        // Buscar itens aprovados e editados
        var todosItens = await _itemRepo.FindAsync(
            i => i.DocumentoLensId == r.ProcessamentoId, ct);

        var itensParaConsolidar = todosItens
            .Where(i => i.Status == StatusItemLensEnum.Aprovado || i.Status == StatusItemLensEnum.Editado)
            .ToList();

        if (itensParaConsolidar.Count == 0)
            throw new InvalidOperationException("Nenhum item aprovado ou editado para consolidar.");

        var itensRejeitados = todosItens.Count(i => i.Status == StatusItemLensEnum.Rejeitado);

        await _uow.BeginTransactionAsync(ct);
        try
        {
            var numero = $"LN-{DateTime.UtcNow:yyMMddHHmmss}";

            var pedido = new PedidoCompra
            {
                EmpresaId = r.EmpresaId,
                ObraId = r.Dto.ObraId,
                FornecedorId = r.Dto.FornecedorId,
                Numero = numero,
                Status = StatusPedidoCompraEnum.Rascunho,
                DataPedido = DateTime.UtcNow,
                Observacoes = r.Dto.Observacoes ?? $"Consolidado via Lens - Documento {documento.NomeArquivo}",
                CriadoPor = r.UsuarioId
            };

            foreach (var item in itensParaConsolidar)
            {
                var unidade = item.Unidade ?? "UN";
                if (unidade.Length > 20) unidade = unidade[..20];

                pedido.Itens.Add(new ItemPedidoCompra
                {
                    EmpresaId = r.EmpresaId,
                    Descricao = item.Descricao.Length > 500 ? item.Descricao[..500] : item.Descricao,
                    UnidadeMedida = unidade,
                    QuantidadePedida = item.Quantidade ?? 1,
                    PrecoUnitario = item.PrecoUnitario ?? 0
                });
            }

            pedido.ValorTotal = pedido.Itens.Sum(i => i.QuantidadePedida * i.PrecoUnitario);

            await _comprasRepo.AddPedidoAsync(pedido, ct);

            documento.CompraId = pedido.Id;
            documento.Status = StatusProcessamentoLensEnum.Consolidado;
            documento.FornecedorId = r.Dto.FornecedorId;
            documento.UpdatedAt = DateTime.UtcNow;
            documento.UpdatedBy = r.UsuarioId;
            _docRepo.Update(documento);

            await _uow.SaveChangesAsync(ct);
            await _uow.CommitTransactionAsync(ct);

            await _bus.Publish(new DocumentoLensConsolidatedEvent
            {
                ProcessamentoId = documento.Id,
                CompraId = pedido.Id,
                UsuarioId = r.UsuarioId,
                ObraId = r.Dto.ObraId,
                EmpresaId = r.EmpresaId,
                TotalItensConsolidados = itensParaConsolidar.Count,
                TotalItensRejeitados = itensRejeitados,
                ValorTotal = pedido.ValorTotal
            }, ct);

            return pedido.Id;
        }
        catch
        {
            await _uow.RollbackTransactionAsync(ct);
            throw;
        }
    }
}
