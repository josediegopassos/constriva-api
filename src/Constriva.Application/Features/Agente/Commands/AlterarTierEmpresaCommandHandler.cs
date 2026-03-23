using MediatR;
using Constriva.Domain.Entities.Agente;
using Constriva.Domain.Interfaces.Repositories;

namespace Constriva.Application.Features.Agente.Commands;

public record AlterarTierEmpresaCommand(Guid EmpresaId, Guid TierId)
    : IRequest<Unit>;

public class AlterarTierEmpresaCommandHandler : IRequestHandler<AlterarTierEmpresaCommand, Unit>
{
    private readonly IAgenteRepository _repo;
    private readonly IUnitOfWork _uow;
    public AlterarTierEmpresaCommandHandler(IAgenteRepository repo, IUnitOfWork uow)
    { _repo = repo; _uow = uow; }

    public async Task<Unit> Handle(AlterarTierEmpresaCommand r, CancellationToken ct)
    {
        var config = await _repo.GetEmpresaConfigAsync(r.EmpresaId, ct);

        if (config == null)
        {
            config = new AgenteEmpresaConfig
            {
                EmpresaId = r.EmpresaId,
                AgenteTierId = r.TierId,
                Ativo = true,
                DataAtivacao = DateTime.UtcNow
            };
            await _repo.AddEmpresaConfigAsync(config, ct);
        }
        else
        {
            config.AgenteTierId = r.TierId;
        }

        await _uow.SaveChangesAsync(ct);

        return Unit.Value;
    }
}
