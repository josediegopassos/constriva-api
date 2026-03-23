using MediatR;
using Constriva.Application.Common.Behaviors;
using Constriva.Domain.Interfaces.Repositories;
using Constriva.Application.Features.Fornecedores.DTOs;

namespace Constriva.Application.Features.Fornecedores.Queries;

public record GetFornecedoresAtivosQuery(Guid EmpresaId)
    : IRequest<IEnumerable<FornecedorAtivoDto>>, ITenantRequest;

public class GetFornecedoresAtivosHandler : IRequestHandler<GetFornecedoresAtivosQuery, IEnumerable<FornecedorAtivoDto>>
{
    private readonly IFornecedorRepository _repo;
    public GetFornecedoresAtivosHandler(IFornecedorRepository repo) => _repo = repo;

    public async Task<IEnumerable<FornecedorAtivoDto>> Handle(GetFornecedoresAtivosQuery r, CancellationToken ct)
    {
        var fornecedores = await _repo.GetAllByEmpresaAsync(r.EmpresaId, ct);
        return fornecedores
            .Where(f => f.Ativo)
            .OrderBy(f => f.RazaoSocial)
            .Select(f => new FornecedorAtivoDto(f.Id, f.RazaoSocial, f.NomeFantasia));
    }
}
