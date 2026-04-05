using MediatR;
using Constriva.Application.Common.Behaviors;
using Constriva.Domain.Interfaces.Repositories;
using Constriva.Application.Features.Compras.DTOs;

namespace Constriva.Application.Features.Compras;

public record GetFornecedoresCotacaoQuery(Guid CotacaoId, Guid EmpresaId)
    : IRequest<IEnumerable<FornecedorCotacaoDto>>, ITenantRequest;

public class GetFornecedoresCotacaoHandler
    : IRequestHandler<GetFornecedoresCotacaoQuery, IEnumerable<FornecedorCotacaoDto>>
{
    private readonly IComprasRepository _repo;
    public GetFornecedoresCotacaoHandler(IComprasRepository repo) => _repo = repo;

    public async Task<IEnumerable<FornecedorCotacaoDto>> Handle(
        GetFornecedoresCotacaoQuery request, CancellationToken ct)
    {
        var fornecedores = await _repo.GetFornecedoresCotacaoAsync(
            request.CotacaoId, request.EmpresaId, ct);

        return fornecedores.Select(f => new FornecedorCotacaoDto(
            f.Id, f.FornecedorId,
            f.Fornecedor?.NomeFantasia ?? f.Fornecedor?.RazaoSocial ?? "",
            f.Fornecedor?.Documento,
            f.Fornecedor?.Email,
            f.Status, f.ConvidadoEm, f.RespondeuEm));
    }
}
