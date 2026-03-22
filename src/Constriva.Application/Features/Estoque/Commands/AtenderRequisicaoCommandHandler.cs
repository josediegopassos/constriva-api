using MediatR;
using Constriva.Application.Common.Behaviors;
using Constriva.Application.Common.Interfaces;
using Constriva.Domain.Entities.Estoque;
using Constriva.Domain.Enums;
using Constriva.Domain.Interfaces.Repositories;
using Constriva.Application.Features.Estoque.DTOs;

namespace Constriva.Application.Features.Estoque.Commands;

public record AtenderItemDto(Guid ItemRequisicaoId, decimal QuantidadeAtendida);

public record AtenderRequisicaoCommand(Guid Id, Guid EmpresaId, Guid UsuarioId, IEnumerable<AtenderItemDto> Itens)
    : IRequest<RequisicaoDto>, ITenantRequest { public Guid TenantId => EmpresaId; }

public class AtenderRequisicaoHandler : IRequestHandler<AtenderRequisicaoCommand, RequisicaoDto>
{
    private readonly IEstoqueRepository _repo;
    private readonly IUnitOfWork _uow;

    public AtenderRequisicaoHandler(IEstoqueRepository repo, IUnitOfWork uow)
    {
        _repo = repo;
        _uow = uow;
    }

    public async Task<RequisicaoDto> Handle(AtenderRequisicaoCommand request, CancellationToken cancellationToken)
    {
        var requisicao = await _repo.GetRequisicaoByIdAsync(request.Id, request.EmpresaId, cancellationToken)
            ?? throw new KeyNotFoundException($"Requisição {request.Id} não encontrada.");

        if (requisicao.Status != StatusRequisicaoEnum.Aprovada)
            throw new InvalidOperationException($"Apenas requisições aprovadas podem ser atendidas. Status atual: '{requisicao.Status}'.");

        var almoxarifado = await _repo.GetAlmoxarifadoByIdAsync(requisicao.AlmoxarifadoId, request.EmpresaId, cancellationToken);
        var almoxarifadoNome = almoxarifado?.Nome ?? requisicao.AlmoxarifadoId.ToString();

        foreach (var atenderItem in request.Itens)
        {
            var item = requisicao.Itens.FirstOrDefault(i => i.Id == atenderItem.ItemRequisicaoId)
                ?? throw new KeyNotFoundException($"Item {atenderItem.ItemRequisicaoId} não pertence a esta requisição.");

            if (atenderItem.QuantidadeAtendida <= 0)
                throw new InvalidOperationException($"Quantidade atendida do item {atenderItem.ItemRequisicaoId} deve ser maior que zero.");

            if (atenderItem.QuantidadeAtendida > item.QuantidadeSolicitada - item.QuantidadeAtendida)
                throw new InvalidOperationException(
                    $"Quantidade atendida ({atenderItem.QuantidadeAtendida}) excede o saldo pendente do item ({item.QuantidadeSolicitada - item.QuantidadeAtendida}).");

            var saldo = await _repo.GetSaldoAsync(requisicao.AlmoxarifadoId, item.MaterialId, request.EmpresaId, cancellationToken);

            if (saldo == null || saldo.SaldoDisponivel < atenderItem.QuantidadeAtendida)
                throw new InvalidOperationException(
                    $"Saldo insuficiente para o material '{item.Material?.Nome ?? item.MaterialId.ToString()}' " +
                    $"no almoxarifado '{almoxarifadoNome}'. " +
                    $"Disponível: {saldo?.SaldoDisponivel ?? 0}, Solicitado: {atenderItem.QuantidadeAtendida}.");

            var saldoAnterior = saldo.SaldoAtual;
            saldo.SaldoAtual -= atenderItem.QuantidadeAtendida;
            saldo.UltimaMovimentacao = DateTime.UtcNow;

            await _repo.AddMovimentacaoAsync(new MovimentacaoEstoque
            {
                EmpresaId = request.EmpresaId,
                AlmoxarifadoId = requisicao.AlmoxarifadoId,
                MaterialId = item.MaterialId,
                ObraId = requisicao.ObraId,
                FaseObraId = requisicao.FaseObraId,
                Tipo = TipoMovimentacaoEstoqueEnum.Saida,
                Quantidade = atenderItem.QuantidadeAtendida,
                PrecoUnitario = item.PrecoReferencia ?? saldo.CustoMedio,
                SaldoAnterior = saldoAnterior,
                SaldoPosterior = saldo.SaldoAtual,
                DataMovimentacao = DateTime.UtcNow,
                NumeroDocumento = requisicao.Numero,
                Observacao = $"Atendimento da requisição {requisicao.Numero}",
                UsuarioId = request.UsuarioId
            }, cancellationToken);

            item.QuantidadeAtendida += atenderItem.QuantidadeAtendida;
        }

        requisicao.Status = StatusRequisicaoEnum.Atendida;
        await _uow.SaveChangesAsync(cancellationToken);

        return CreateRequisicaoHandler.ToDto(requisicao);
    }
}
