using MediatR;
using Constriva.Application.Common.Behaviors;
using Constriva.Domain.Entities.Compras;
using Constriva.Domain.Enums;
using Constriva.Domain.Interfaces.Repositories;
using Constriva.Application.Features.Compras.DTOs;

namespace Constriva.Application.Features.Compras.Commands;

public record UpdatePropostaCotacaoCommand(
    Guid PropostaId,
    Guid EmpresaId,
    UpdatePropostaDto Dto) : IRequest<PropostaDto>, ITenantRequest;

public class UpdatePropostaCotacaoHandler : IRequestHandler<UpdatePropostaCotacaoCommand, PropostaDto>
{
    private readonly IComprasRepository _repo;
    private readonly IUnitOfWork _uow;

    public UpdatePropostaCotacaoHandler(IComprasRepository repo, IUnitOfWork uow)
    {
        _repo = repo;
        _uow = uow;
    }

    public async Task<PropostaDto> Handle(UpdatePropostaCotacaoCommand request, CancellationToken ct)
    {
        var proposta = await _repo.GetPropostaByIdAsync(request.PropostaId, request.EmpresaId, ct)
            ?? throw new KeyNotFoundException($"Proposta {request.PropostaId} não encontrada.");

        var cotacao = await _repo.GetCotacaoByIdAsync(proposta.CotacaoId, request.EmpresaId, ct)
            ?? throw new KeyNotFoundException($"Cotação {proposta.CotacaoId} não encontrada.");

        if (cotacao.Status != StatusCotacaoEnum.Aberta)
            throw new InvalidOperationException(
                $"Cotação no status '{cotacao.Status}' não permite edição de propostas.");

        var dto = request.Dto;

        if (dto.DataValidade.HasValue) proposta.DataValidade = dto.DataValidade;
        if (dto.CondicoesPagamento is not null) proposta.CondicoesPagamento = dto.CondicoesPagamento;
        if (dto.PrazoEntrega.HasValue) proposta.PrazoEntrega = dto.PrazoEntrega;
        if (dto.Observacoes is not null) proposta.Observacoes = dto.Observacoes;

        // Salvar alterações nos campos antes de lidar com itens
        await _uow.SaveChangesAsync(ct);

        if (dto.Itens is not null)
        {
            var novosItens = dto.Itens.Select(item => new ItemPropostaCotacao
            {
                EmpresaId = request.EmpresaId,
                PropostaId = proposta.Id,
                ItemCotacaoId = item.ItemCotacaoId,
                PrecoUnitario = item.PrecoUnitario,
                Quantidade = item.Quantidade,
                Marca = item.Marca,
                Observacao = item.Observacao,
                Disponivel = item.Disponivel
            }).ToList();

            await _repo.ReplaceItensPropostaAsync(proposta.Id, novosItens, ct);
        }

        // Recarregar para retornar dados atualizados
        var atualizada = await _repo.GetPropostaByIdAsync(request.PropostaId, request.EmpresaId, ct);

        // Recalcular valor total
        atualizada!.ValorTotal = atualizada.Itens
            .Where(i => !i.IsDeleted && i.Disponivel)
            .Sum(i => i.PrecoUnitario * i.Quantidade);

        await _uow.SaveChangesAsync(ct);

        return PropostaMapper.ToDto(atualizada);
    }
}
