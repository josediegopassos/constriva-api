using MediatR;
using Constriva.Application.Common.Behaviors;
using Constriva.Domain.Enums;
using Constriva.Domain.Interfaces.Repositories;

namespace Constriva.Application.Features.Contratos.Commands;

public record AlterarStatusContratoCommand(Guid Id, Guid EmpresaId, StatusContratoEnum NovoStatus)
    : IRequest<Unit>, ITenantRequest { public Guid TenantId => EmpresaId; }

public class AlterarStatusContratoHandler : IRequestHandler<AlterarStatusContratoCommand, Unit>
{
    private readonly IContratoRepository _repo;
    private readonly IUnitOfWork _uow;

    public AlterarStatusContratoHandler(IContratoRepository repo, IUnitOfWork uow)
    { _repo = repo; _uow = uow; }

    public async Task<Unit> Handle(AlterarStatusContratoCommand r, CancellationToken ct)
    {
        var contrato = await _repo.GetByIdAndEmpresaAsync(r.Id, r.EmpresaId, ct)
            ?? throw new KeyNotFoundException($"Contrato {r.Id} não encontrado.");

        if (contrato.Status == r.NovoStatus)
            throw new InvalidOperationException($"Contrato já está com status '{r.NovoStatus}'.");

        contrato.Status = r.NovoStatus;

        if (r.NovoStatus == StatusContratoEnum.Encerrado || r.NovoStatus == StatusContratoEnum.Rescindido)
            contrato.DataEncerramento = DateTime.UtcNow;

        _repo.Update(contrato);
        await _uow.SaveChangesAsync(ct);
        return Unit.Value;
    }
}
