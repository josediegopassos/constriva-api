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

public record UpdateAtividadeCommand(Guid Id, Guid EmpresaId, string Nome, string? Descricao,
    DateTime DataInicio, DateTime DataFim, decimal PercentualConcluido, string? Responsavel)
    : IRequest<AtividadeDto>, ITenantRequest { public Guid TenantId => EmpresaId; }

public class UpdateAtividadeCommandHandler : IRequestHandler<UpdateAtividadeCommand, AtividadeDto>
{
    private readonly ICronogramaRepository _repo;
    private readonly IUnitOfWork _uow;

    public UpdateAtividadeCommandHandler(ICronogramaRepository repo, IUnitOfWork uow)
    {
        _repo = repo;
        _uow = uow;
    }

    public async Task<AtividadeDto> Handle(UpdateAtividadeCommand request, CancellationToken cancellationToken)
    {
        var atividade = await _repo.GetAtividadeByIdAsync(request.Id, request.EmpresaId, cancellationToken)
            ?? throw new KeyNotFoundException("Atividade não encontrada.");

        atividade.Nome = request.Nome;
        atividade.Descricao = request.Descricao;
        atividade.DataInicioPlanejada = request.DataInicio;
        atividade.DataFimPlanejada = request.DataFim;
        atividade.PercentualConcluido = request.PercentualConcluido;
        atividade.Status = request.PercentualConcluido == 100
            ? StatusAtividadeEnum.Concluida
            : request.PercentualConcluido > 0
                ? StatusAtividadeEnum.EmAndamento
                : StatusAtividadeEnum.NaoIniciada;

        await _uow.SaveChangesAsync(cancellationToken);

        return new AtividadeDto(
            atividade.Id, atividade.Nome, atividade.Descricao, atividade.Ordem,
            atividade.DataInicioPlanejada, atividade.DataFimPlanejada,
            null, null, atividade.PercentualConcluido, atividade.Status,
            false, Enumerable.Empty<Guid>());
    }
}
