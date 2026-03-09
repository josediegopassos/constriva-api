using MediatR;
using Constriva.Application.Common.Behaviors;
using Constriva.Application.Common.Interfaces;
using Constriva.Domain.Entities.Orcamento;
using Constriva.Domain.Enums;
using Constriva.Domain.Interfaces.Repositories;
using Constriva.Application.Features.Orcamento.DTOs;

namespace Constriva.Application.Features.Orcamento.Commands;

public record ReprovarOrcamentoCommand(
    Guid Id,
    Guid EmpresaId,
    Guid UsuarioId,
    string Motivo) : IRequest<Unit>, ITenantRequest;

public class ReprovarOrcamentoHandler : IRequestHandler<ReprovarOrcamentoCommand, Unit>
{
    private readonly IOrcamentoRepository _repo;
    private readonly IUnitOfWork _uow;

    public ReprovarOrcamentoHandler(IOrcamentoRepository repo, IUnitOfWork uow)
    {
        _repo = repo;
        _uow = uow;
    }

    public async Task<Unit> Handle(ReprovarOrcamentoCommand request, CancellationToken ct)
    {
        var orcamento = await _repo.GetByIdAsync(request.Id, request.EmpresaId, ct)
            ?? throw new KeyNotFoundException("Orçamento não encontrado.");

        if (orcamento.Status != StatusOrcamentoEnum.EmAnalise)
            throw new InvalidOperationException(
                $"Orçamento no status '{orcamento.Status}' não pode ser reprovado. Deve estar em análise.");

        orcamento.Status = StatusOrcamentoEnum.Reprovado;
        orcamento.Observacoes = request.Motivo;
        _repo.Update(orcamento);

        var historico = new OrcamentoHistorico
        {
            EmpresaId = request.EmpresaId,
            OrcamentoId = orcamento.Id,
            Descricao = $"Orçamento reprovado. Motivo: {request.Motivo}",
            UsuarioId = request.UsuarioId
        };
        await _repo.AddHistoricoAsync(historico, ct);
        await _uow.SaveChangesAsync(ct);
        return Unit.Value;
    }
}
