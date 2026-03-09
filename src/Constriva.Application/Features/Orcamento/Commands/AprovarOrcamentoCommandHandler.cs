using FluentValidation;
using MediatR;
using Constriva.Application.Common.Behaviors;
using Constriva.Application.Common.Interfaces;
using Constriva.Domain.Entities.Orcamento;
using Constriva.Domain.Enums;
using Constriva.Domain.Interfaces.Repositories;
using Constriva.Application.Features.Orcamento.DTOs;

namespace Constriva.Application.Features.Orcamento.Commands;

public record AprovarOrcamentoCommand(Guid Id, Guid EmpresaId, Guid UsuarioId, string? Observacao)
    : IRequest<bool>, ITenantRequest;

public class AprovarOrcamentoHandler : IRequestHandler<AprovarOrcamentoCommand, bool>
{
    private readonly IOrcamentoRepository _repo;
    private readonly IUnitOfWork _uow;

    public AprovarOrcamentoHandler(IOrcamentoRepository repo, IUnitOfWork uow)
    {
        _repo = repo;
        _uow = uow;
    }

    public async Task<bool> Handle(AprovarOrcamentoCommand request, CancellationToken ct)
    {
        var orcamento = await _repo.GetByIdAsync(request.Id, request.EmpresaId, ct)
            ?? throw new KeyNotFoundException("Orçamento não encontrado.");

        if (orcamento.Status == StatusOrcamentoEnum.Aprovado)
            return true; // idempotente

        if (orcamento.Status != StatusOrcamentoEnum.EmAnalise)
            throw new InvalidOperationException(
                $"Orçamento não pode ser aprovado no status '{orcamento.Status}'. Envie-o para análise primeiro.");

        if (orcamento.ValorTotal == 0)
            throw new InvalidOperationException("Não é possível aprovar orçamento com valor zero.");

        orcamento.Status = StatusOrcamentoEnum.Aprovado;
        orcamento.AprovadoPor = request.UsuarioId;
        orcamento.DataAprovacao = DateTime.UtcNow;
        orcamento.Observacoes = request.Observacao;

        _repo.Update(orcamento);

        await _repo.AddHistoricoAsync(new OrcamentoHistorico
        {
            EmpresaId = request.EmpresaId,
            OrcamentoId = orcamento.Id,
            Descricao = "Orçamento aprovado.",
            UsuarioId = request.UsuarioId
        }, ct);
        await _uow.SaveChangesAsync(ct);

        return true;
    }
}
