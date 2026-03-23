using MediatR;
using Constriva.Application.Common.Behaviors;
using Constriva.Domain.Interfaces.Repositories;
using Constriva.Application.Features.Fornecedores.DTOs;
using Constriva.Application.Features.Fornecedores.Commands;

namespace Constriva.Application.Features.Fornecedores.Queries;

public record GetFornecedorByIdQuery(Guid Id, Guid EmpresaId)
    : IRequest<FornecedorDto?>, ITenantRequest;

public class GetFornecedorByIdHandler : IRequestHandler<GetFornecedorByIdQuery, FornecedorDto?>
{
    private readonly IFornecedorRepository _repo;
    public GetFornecedorByIdHandler(IFornecedorRepository repo) => _repo = repo;

    public async Task<FornecedorDto?> Handle(GetFornecedorByIdQuery r, CancellationToken ct)
    {
        var f = await _repo.GetByIdComEnderecoAsync(r.Id, r.EmpresaId, ct);
        if (f == null || f.IsDeleted) return null;
        return CreateFornecedorCommandHandler.ToDto(f);
    }
}
