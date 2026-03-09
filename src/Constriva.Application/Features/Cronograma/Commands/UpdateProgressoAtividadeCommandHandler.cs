// ─── CRONOGRAMA ───────────────────────────────────────────────────────────────
using FluentValidation;
using MediatR;
using Constriva.Application.Common.Behaviors;
using Constriva.Application.Common.Interfaces;
using Constriva.Domain.Entities.Cronograma;
using Constriva.Domain.Enums;
using Constriva.Domain.Interfaces.Repositories;
using Constriva.Application.Features.Cronograma.DTOs;

namespace Constriva.Application.Features.Cronograma.Commands;

public record UpdateProgressoAtividadeCommand(Guid Id, Guid EmpresaId, decimal Percentual)
    : IRequest<Unit>, ITenantRequest;

public class UpdateProgressoAtividadeCommandHandler : IRequestHandler<UpdateProgressoAtividadeCommand, Unit>
{
    private readonly ICronogramaRepository _repo;
    private readonly IUnitOfWork _uow;

    public UpdateProgressoAtividadeCommandHandler(ICronogramaRepository repo, IUnitOfWork uow)
    {
        _repo = repo;
        _uow = uow;
    }

    public async Task<Unit> Handle(UpdateProgressoAtividadeCommand request, CancellationToken cancellationToken)
    {
        if (request.Percentual > 100)
            throw new InvalidOperationException("Percentual não pode ser maior que 100.");

        var atividade = await _repo.GetAtividadeByIdAsync(request.Id, request.EmpresaId, cancellationToken);
        if (atividade == null)
            throw new KeyNotFoundException("Atividade não encontrada.");

        atividade.PercentualConcluido = request.Percentual;
        atividade.Status = request.Percentual == 100
            ? StatusAtividadeEnum.Concluida
            : request.Percentual > 0
                ? StatusAtividadeEnum.EmAndamento
                : StatusAtividadeEnum.NaoIniciada;

        if (request.Percentual > 0 && atividade.DataInicioReal == null)
            atividade.DataInicioReal = DateTime.Today;
        if (request.Percentual >= 100 && atividade.DataFimReal == null)
            atividade.DataFimReal = DateTime.Today;

        await _uow.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}
