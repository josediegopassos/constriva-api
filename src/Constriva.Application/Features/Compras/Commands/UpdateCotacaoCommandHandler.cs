using MediatR;
using Constriva.Application.Common.Behaviors;
using Constriva.Domain.Entities.Compras;
using Constriva.Domain.Enums;
using Constriva.Domain.Interfaces.Repositories;
using Constriva.Application.Features.Compras.DTOs;

namespace Constriva.Application.Features.Compras.Commands;

public record UpdateCotacaoCommand(Guid Id, Guid EmpresaId, UpdateCotacaoDto Dto)
    : IRequest<CotacaoDto>, ITenantRequest;

public class UpdateCotacaoHandler : IRequestHandler<UpdateCotacaoCommand, CotacaoDto>
{
    private readonly IComprasRepository _repo;
    private readonly IUnitOfWork _uow;

    public UpdateCotacaoHandler(IComprasRepository repo, IUnitOfWork uow)
    {
        _repo = repo;
        _uow = uow;
    }

    public async Task<CotacaoDto> Handle(UpdateCotacaoCommand request, CancellationToken ct)
    {
        var cotacao = await _repo.GetCotacaoByIdAsync(request.Id, request.EmpresaId, ct)
            ?? throw new KeyNotFoundException($"Cotação {request.Id} não encontrada.");

        var dto = request.Dto;

        // Permite edição de campos apenas em cotações abertas (exceto mudança de status)
        if (cotacao.Status != StatusCotacaoEnum.Aberta && dto.Status is null)
            throw new InvalidOperationException(
                $"Cotação no status '{cotacao.Status}' não pode ser editada. Apenas cotações abertas podem ser alteradas.");

        if (dto.ObraId.HasValue) cotacao.ObraId = dto.ObraId.Value;
        if (dto.Titulo is not null) cotacao.Titulo = dto.Titulo;
        if (dto.Status.HasValue) cotacao.Status = dto.Status.Value;
        if (dto.DataFechamento.HasValue) cotacao.DataFechamento = dto.DataFechamento;
        if (dto.DataLimiteResposta.HasValue) cotacao.DataLimiteResposta = dto.DataLimiteResposta;
        if (dto.Observacoes is not null) cotacao.Observacoes = dto.Observacoes;
        if (dto.CondicoesGerais is not null) cotacao.CondicoesGerais = dto.CondicoesGerais;
        if (dto.FornecedorVencedorId.HasValue) cotacao.FornecedorVencedorId = dto.FornecedorVencedorId;

        // Salvar alterações nos campos da cotação antes de lidar com itens
        await _uow.SaveChangesAsync(ct);

        if (dto.Itens is not null)
        {
            // Substituir itens via repositório (SQL direto + novos itens)
            var novosItens = dto.Itens.Select((item, idx) => new ItemCotacao
            {
                EmpresaId = request.EmpresaId,
                CotacaoId = cotacao.Id,
                MaterialId = item.MaterialId,
                Descricao = item.Descricao,
                UnidadeMedida = item.UnidadeMedida,
                Quantidade = item.Quantidade,
                Especificacao = item.Especificacao,
                PrecoReferencia = item.PrecoReferencia,
                Ordem = idx + 1
            }).ToList();

            await _repo.ReplaceItensCotacaoAsync(cotacao.Id, novosItens, ct);
        }

        var atualizada = await _repo.GetCotacaoByIdAsync(request.Id, request.EmpresaId, ct);
        return CotacaoMapper.ToDto(atualizada!);
    }
}
