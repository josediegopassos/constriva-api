using MediatR;
using Constriva.Application.Common.Behaviors;
using Constriva.Domain.Interfaces.Repositories;
using Constriva.Application.Features.Qualidade.DTOs;

namespace Constriva.Application.Features.Qualidade;

public record GetFVSQuery(Guid EmpresaId, Guid? ObraId = null)
    : IRequest<IEnumerable<FVSDto>>, ITenantRequest;

public class GetFVSHandler : IRequestHandler<GetFVSQuery, IEnumerable<FVSDto>>
{
    private readonly IQualidadeRepository _repo;
    public GetFVSHandler(IQualidadeRepository repo) => _repo = repo;

    public async Task<IEnumerable<FVSDto>> Handle(GetFVSQuery r, CancellationToken ct)
    {
        var items = await _repo.GetFVSsAsync(r.EmpresaId, r.ObraId, ct);
        return items.Select(f => new FVSDto(
            f.Id, f.ObraId, f.Numero, f.Servico, f.Aprovado, f.DataVerificacao, f.ResponsavelId));
    }
}
