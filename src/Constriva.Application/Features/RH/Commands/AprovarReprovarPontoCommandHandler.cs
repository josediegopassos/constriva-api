using MediatR;
using Constriva.Application.Common.Behaviors;
using Constriva.Domain.Enums;
using Constriva.Domain.Interfaces.Repositories;

namespace Constriva.Application.Features.RH.Commands;

public record AprovarPontoCommand(Guid Id, Guid EmpresaId, Guid UserId)
    : IRequest<Unit>, ITenantRequest { public Guid TenantId => EmpresaId; }

public record ReprovarPontoCommand(Guid Id, Guid EmpresaId, Guid UserId)
    : IRequest<Unit>, ITenantRequest { public Guid TenantId => EmpresaId; }

public class AprovarPontoHandler : IRequestHandler<AprovarPontoCommand, Unit>
{
    private readonly IRHRepository _repo;
    private readonly IUnitOfWork _uow;

    public AprovarPontoHandler(IRHRepository repo, IUnitOfWork uow)
    {
        _repo = repo;
        _uow = uow;
    }

    public async Task<Unit> Handle(AprovarPontoCommand request, CancellationToken cancellationToken)
    {
        var ponto = await _repo.GetPontoByIdAsync(request.Id, request.EmpresaId, cancellationToken)
            ?? throw new KeyNotFoundException($"Registro de ponto {request.Id} não encontrado.");

        if (ponto.StatusAprovacao == StatusAprovacaoPontoEnum.Aprovado)
            throw new InvalidOperationException("Este ponto já foi aprovado.");

        ponto.StatusAprovacao = StatusAprovacaoPontoEnum.Aprovado;
        ponto.AprovadoPor = request.UserId;
        ponto.ReprovadoPor = null;

        await _uow.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}

public class ReprovarPontoHandler : IRequestHandler<ReprovarPontoCommand, Unit>
{
    private readonly IRHRepository _repo;
    private readonly IUnitOfWork _uow;

    public ReprovarPontoHandler(IRHRepository repo, IUnitOfWork uow)
    {
        _repo = repo;
        _uow = uow;
    }

    public async Task<Unit> Handle(ReprovarPontoCommand request, CancellationToken cancellationToken)
    {
        var ponto = await _repo.GetPontoByIdAsync(request.Id, request.EmpresaId, cancellationToken)
            ?? throw new KeyNotFoundException($"Registro de ponto {request.Id} não encontrado.");

        if (ponto.StatusAprovacao == StatusAprovacaoPontoEnum.Reprovado)
            throw new InvalidOperationException("Este ponto já foi reprovado.");

        ponto.StatusAprovacao = StatusAprovacaoPontoEnum.Reprovado;
        ponto.ReprovadoPor = request.UserId;
        ponto.AprovadoPor = null;

        await _uow.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}
