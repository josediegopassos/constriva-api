using MediatR;
using Constriva.Application.Common.Behaviors;
using Constriva.Domain.Entities.Compras;
using Constriva.Domain.Enums;
using Constriva.Domain.Interfaces.Repositories;
using Constriva.Application.Features.Compras.DTOs;

namespace Constriva.Application.Features.Compras.Commands;

public record CreatePropostaCotacaoCommand(
    Guid CotacaoId,
    Guid EmpresaId,
    CreatePropostaDto Dto) : IRequest<PropostaDto>, ITenantRequest;

public class CreatePropostaCotacaoHandler : IRequestHandler<CreatePropostaCotacaoCommand, PropostaDto>
{
    private readonly IComprasRepository _repo;
    private readonly IUnitOfWork _uow;

    public CreatePropostaCotacaoHandler(IComprasRepository repo, IUnitOfWork uow)
    {
        _repo = repo;
        _uow = uow;
    }

    public async Task<PropostaDto> Handle(CreatePropostaCotacaoCommand request, CancellationToken ct)
    {
        var cotacao = await _repo.GetCotacaoByIdAsync(request.CotacaoId, request.EmpresaId, ct)
            ?? throw new KeyNotFoundException($"Cotação {request.CotacaoId} não encontrada.");

        if (cotacao.Status != StatusCotacaoEnum.Aberta)
            throw new InvalidOperationException(
                $"Cotação no status '{cotacao.Status}' não aceita propostas. Apenas cotações abertas podem receber propostas.");

        var dto = request.Dto;

        var fc = await _repo.GetFornecedorCotacaoAsync(request.CotacaoId, dto.FornecedorId, request.EmpresaId, ct)
            ?? throw new InvalidOperationException(
                $"Fornecedor {dto.FornecedorId} não foi convidado para a cotação {request.CotacaoId}.");

        var proposta = new PropostaCotacao
        {
            EmpresaId = request.EmpresaId,
            CotacaoId = request.CotacaoId,
            FornecedorId = dto.FornecedorId,
            DataRecebimento = DateTime.UtcNow,
            DataValidade = dto.DataValidade,
            CondicoesPagamento = dto.CondicoesPagamento,
            PrazoEntrega = dto.PrazoEntrega,
            Observacoes = dto.Observacoes
        };

        foreach (var item in dto.Itens ?? Enumerable.Empty<CreateItemPropostaDto>())
        {
            proposta.Itens.Add(new ItemPropostaCotacao
            {
                EmpresaId = request.EmpresaId,
                PropostaId = proposta.Id,
                ItemCotacaoId = item.ItemCotacaoId,
                PrecoUnitario = item.PrecoUnitario,
                Quantidade = item.Quantidade,
                Marca = item.Marca,
                Observacao = item.Observacao,
                Disponivel = item.Disponivel
            });
        }

        // Calcular valor total a partir dos itens
        proposta.ValorTotal = proposta.Itens
            .Where(i => i.Disponivel)
            .Sum(i => i.PrecoUnitario * i.Quantidade);

        // Atualizar status do convite
        fc.Status = StatusConviteCotacaoEnum.Respondido;
        fc.RespondeuEm = DateTime.UtcNow;

        await _repo.AddPropostaAsync(proposta, ct);
        await _uow.SaveChangesAsync(ct);

        // Recarregar para obter dados de navegação
        var criada = await _repo.GetPropostaByIdAsync(proposta.Id, request.EmpresaId, ct);
        return PropostaMapper.ToDto(criada!);
    }
}
