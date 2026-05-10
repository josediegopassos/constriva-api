using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Constriva.Domain.Entities.Compras;
using Constriva.Domain.Enums;
using Constriva.Domain.Interfaces.WhatsApp;
using Constriva.Infrastructure.Integrations.WhatsApp;
using Constriva.Infrastructure.Persistence;
using Constriva.Messaging.Contracts.WhatsApp.Events;

namespace Constriva.Messaging.Consumers.WhatsApp;

public class CotacaoAprovadaConsumer : IConsumer<CotacaoAprovadaEvent>
{
    private readonly AppDbContext _db;
    private readonly IWhatsAppGateway _gateway;
    private readonly IPublishEndpoint _publish;
    private readonly ILogger<CotacaoAprovadaConsumer> _logger;

    public CotacaoAprovadaConsumer(
        AppDbContext db, IWhatsAppGateway gateway,
        IPublishEndpoint publish, ILogger<CotacaoAprovadaConsumer> logger)
    {
        _db = db;
        _gateway = gateway;
        _publish = publish;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<CotacaoAprovadaEvent> context)
    {
        var msg = context.Message;
        var ct = context.CancellationToken;

        _logger.LogInformation(
            "Processando aprovação. Cotação: {Numero} | Fornecedor: {Fornecedor} | Valor: {Valor}",
            msg.NumeroCotacao, msg.NomeFornecedor, msg.ValorTotalAprovado);

        await using var transaction = await _db.Database.BeginTransactionAsync(ct);
        string numeroPedido;
        try
        {
            numeroPedido = await GerarNumeroPedidoAsync(msg.EmpresaId, ct);

            var pedido = new PedidoCompra
            {
                EmpresaId = msg.EmpresaId,
                ObraId = msg.ObraId,
                CotacaoId = msg.CotacaoId,
                FornecedorId = msg.FornecedorId,
                Numero = numeroPedido,
                Status = StatusPedidoCompraEnum.Aprovado,
                DataPedido = DateTime.UtcNow,
                DataEntregaPrevista = msg.DataEntregaPrevista,
                CondicoesPagamento = msg.CondicoesPagamento,
                ValorTotal = msg.ValorTotalAprovado,
                CriadoPor = msg.AprovadoPorUsuarioId,
                AprovadoPor = msg.AprovadoPorUsuarioId,
                DataAprovacao = msg.AprovadaEm
            };

            pedido.Itens = msg.ItensAprovados.Select(i => new ItemPedidoCompra
            {
                EmpresaId = msg.EmpresaId,
                PedidoId = pedido.Id,
                MaterialId = i.MaterialId,
                Descricao = i.Descricao,
                UnidadeMedida = i.UnidadeMedida,
                QuantidadePedida = i.Quantidade,
                PrecoUnitario = i.PrecoUnitario
            }).ToList();

            _db.PedidosCompra.Add(pedido);

            foreach (var item in msg.ItensAprovados.Where(i => i.MaterialId.HasValue))
            {
                var saldo = await _db.EstoquesSaldos
                    .FirstOrDefaultAsync(s =>
                        s.MaterialId == item.MaterialId!.Value &&
                        s.EmpresaId == msg.EmpresaId && !s.IsDeleted, ct);

                if (saldo != null)
                {
                    saldo.SaldoReservado += item.Quantidade;
                    saldo.UltimaMovimentacao = DateTime.UtcNow;
                }
            }

            await _db.SaveChangesAsync(ct);
            await transaction.CommitAsync(ct);

            _logger.LogInformation("PedidoCompra {Numero} criado com {Itens} itens",
                numeroPedido, pedido.Itens.Count);
        }
        catch
        {
            await transaction.RollbackAsync(ct);
            throw;
        }

        try
        {
            if (!string.IsNullOrEmpty(msg.TelefoneFornecedor))
            {
                await _gateway.EnviarConfirmacaoAprovacaoAsync(
                    msg.TelefoneFornecedor, msg.NomeFornecedor, msg.NumeroCotacao,
                    msg.ValorTotalAprovado, msg.PrazoEntregaDias, ct);

                _logger.LogInformation("Confirmação WhatsApp enviada ao fornecedor {F}", msg.NomeFornecedor);
            }
        }
        catch (WhatsAppGatewayException ex)
        {
            _logger.LogWarning(ex,
                "Falha ao enviar confirmação WhatsApp ao fornecedor {F}. Aprovação já processada.",
                msg.NomeFornecedor);
        }

        await _publish.Publish(new WhatsAppAprovacaoProcessadaEvent
        {
            CotacaoId = msg.CotacaoId, EmpresaId = msg.EmpresaId,
            PropostaCotacaoId = msg.PropostaCotacaoId,
            NomeFornecedorVencedor = msg.NomeFornecedor,
            ValorTotalAprovado = msg.ValorTotalAprovado,
            NumeroPedidoCompra = numeroPedido,
            AprovadaEm = msg.AprovadaEm
        }, ct);

        _logger.LogInformation(
            "Aprovação concluída. PedidoCompra: {Pedido} | Fornecedor: {F} | Valor: {V}",
            numeroPedido, msg.NomeFornecedor, msg.ValorTotalAprovado);
    }

    private async Task<string> GerarNumeroPedidoAsync(Guid empresaId, CancellationToken ct)
    {
        var count = await _db.PedidosCompra
            .CountAsync(p => p.EmpresaId == empresaId, ct);
        return $"PED-{DateTime.UtcNow:yyyy}-{(count + 1):D4}";
    }
}
