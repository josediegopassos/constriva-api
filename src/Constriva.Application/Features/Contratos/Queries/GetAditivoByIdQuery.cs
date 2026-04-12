using MediatR;
using Constriva.Application.Common.Behaviors;
using Constriva.Domain.Interfaces.Repositories;
using Constriva.Application.Features.Contratos.DTOs;

namespace Constriva.Application.Features.Contratos.Queries;

public record GetAditivoByIdQuery(Guid Id, Guid EmpresaId)
    : IRequest<AditivoContratoDto?>, ITenantRequest;

public class GetAditivoByIdHandler : IRequestHandler<GetAditivoByIdQuery, AditivoContratoDto?>
{
    private readonly IContratoRepository _repo;
    public GetAditivoByIdHandler(IContratoRepository repo) => _repo = repo;

    public async Task<AditivoContratoDto?> Handle(GetAditivoByIdQuery r, CancellationToken ct)
    {
        var a = await _repo.GetAditivoByIdAsync(r.Id, r.EmpresaId, ct);
        if (a == null) return null;

        return new AditivoContratoDto(
            a.Id, a.ContratoId, a.Numero, a.Tipo,
            a.Justificativa, a.DataAssinatura, a.ValorAditivo,
            a.ProrrogacaoDias, a.NovaDataVigencia, a.ArquivoUrl,
            a.CreatedAt);
    }
}
