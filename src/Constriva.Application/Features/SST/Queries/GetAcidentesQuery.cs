using MediatR;
using Constriva.Application.Common.Behaviors;
using Constriva.Domain.Interfaces.Repositories;
using Constriva.Application.Features.SST.DTOs;

namespace Constriva.Application.Features.SST;

public record GetAcidentesQuery(Guid EmpresaId, Guid? ObraId = null)
    : IRequest<IEnumerable<AcidenteDto>>, ITenantRequest;

public class GetAcidentesHandler : IRequestHandler<GetAcidentesQuery, IEnumerable<AcidenteDto>>
{
    private readonly ISSTRepository _repo;
    public GetAcidentesHandler(ISSTRepository repo) => _repo = repo;

    public async Task<IEnumerable<AcidenteDto>> Handle(GetAcidentesQuery r, CancellationToken ct)
    {
        var items = await _repo.GetAcidentesAsync(r.EmpresaId, r.ObraId, ct);
        return items.Select(a => new AcidenteDto(
            a.Id, a.ObraId, a.Tipo, a.NomeFuncionario,
            a.DescricaoAcidente, a.Local,
            a.AfastamentoMedico, a.DiasAfastamento,
            a.DataHoraAcidente, a.NumeroCAT, a.CreatedAt));
    }
}
