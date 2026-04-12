using MediatR;
using Constriva.Application.Common.Behaviors;
using Constriva.Domain.Interfaces.Repositories;
using Constriva.Application.Features.Contratos.DTOs;

namespace Constriva.Application.Features.Contratos.Queries;

public record GetTodosAditivosQuery(Guid EmpresaId) : IRequest<IEnumerable<AditivoGeralDto>>, ITenantRequest;

public class GetTodosAditivosHandler : IRequestHandler<GetTodosAditivosQuery, IEnumerable<AditivoGeralDto>>
{
    private readonly IContratoRepository _repo;
    public GetTodosAditivosHandler(IContratoRepository repo) => _repo = repo;

    public async Task<IEnumerable<AditivoGeralDto>> Handle(GetTodosAditivosQuery r, CancellationToken ct)
    {
        var aditivos = await _repo.GetTodosAditivosAsync(r.EmpresaId, ct);

        return aditivos.Select(a => new AditivoGeralDto(
            a.Id, a.ContratoId,
            a.Contrato?.Numero ?? "", a.Contrato?.Fornecedor?.RazaoSocial,
            a.Numero, a.Tipo, a.Justificativa,
            a.DataAssinatura, a.ValorAditivo,
            a.ProrrogacaoDias, a.NovaDataVigencia, a.ArquivoUrl,
            a.CreatedAt));
    }
}
