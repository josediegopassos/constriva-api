using MediatR;
using Constriva.Application.Common.Behaviors;
using Constriva.Application.Common.Interfaces;
using Constriva.Domain.Entities.Orcamento;
using Constriva.Domain.Enums;
using Constriva.Domain.Interfaces.Repositories;
using Constriva.Application.Features.Orcamento.DTOs;

namespace Constriva.Application.Features.Orcamento.Commands;

public record EnviarParaRevisaoCommand(
    Guid Id,
    Guid EmpresaId,
    Guid UsuarioId) : IRequest<Unit>, ITenantRequest;

public class EnviarParaRevisaoHandler : IRequestHandler<EnviarParaRevisaoCommand, Unit>
{
    private readonly IOrcamentoRepository _repo;
    private readonly IUnitOfWork _uow;

    public EnviarParaRevisaoHandler(IOrcamentoRepository repo, IUnitOfWork uow)
    {
        _repo = repo;
        _uow = uow;
    }

    public async Task<Unit> Handle(EnviarParaRevisaoCommand request, CancellationToken ct)
    {
        var orcamento = await _repo.GetByIdAsync(request.Id, request.EmpresaId, ct)
            ?? throw new KeyNotFoundException("Orçamento não encontrado.");

        if (orcamento.Status is not (StatusOrcamentoEnum.Rascunho or StatusOrcamentoEnum.Reprovado))
            throw new InvalidOperationException(
                $"Orçamento no status '{orcamento.Status}' não pode ser enviado para análise.");

        orcamento.Status = StatusOrcamentoEnum.EmAnalise;
        _repo.Update(orcamento);

        var historico = new OrcamentoHistorico
        {
            EmpresaId = request.EmpresaId,
            OrcamentoId = orcamento.Id,
            Descricao = "Orçamento enviado para revisão.",
            UsuarioId = request.UsuarioId
        };
        await _repo.AddHistoricoAsync(historico, ct);
        await _uow.SaveChangesAsync(ct);
        return Unit.Value;
    }
}
