using MediatR;
using Constriva.Application.Common.Behaviors;
using Constriva.Domain.Interfaces.Repositories;
using Constriva.Application.Features.Contratos.DTOs;

namespace Constriva.Application.Features.Contratos.Queries;

public record GetAditivosContratoQuery(Guid ContratoId, Guid EmpresaId)
    : IRequest<IEnumerable<AditivoContratoDto>>, ITenantRequest;

public class GetAditivosContratoHandler : IRequestHandler<GetAditivosContratoQuery, IEnumerable<AditivoContratoDto>>
{
    private readonly IContratoRepository _repo;
    public GetAditivosContratoHandler(IContratoRepository repo) => _repo = repo;

    public async Task<IEnumerable<AditivoContratoDto>> Handle(GetAditivosContratoQuery r, CancellationToken ct)
    {
        var aditivos = await _repo.GetAditivosAsync(r.ContratoId, r.EmpresaId, ct);
        return aditivos.Select(a => new AditivoContratoDto(
            a.Id, a.ContratoId, a.Numero, a.Tipo,
            a.Justificativa, a.DataAssinatura, a.ValorAditivo,
            a.ProrrogacaoDias, a.NovaDataVigencia, a.ArquivoUrl,
            a.CreatedAt));
    }
}
