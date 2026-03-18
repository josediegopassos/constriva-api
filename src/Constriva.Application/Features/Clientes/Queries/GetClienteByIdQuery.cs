using MediatR;
using Constriva.Application.Common.Behaviors;
using Constriva.Domain.Interfaces.Repositories;
using Constriva.Application.Features.Clientes.DTOs;

namespace Constriva.Application.Features.Clientes.Queries;

public record GetClienteByIdQuery(Guid Id, Guid EmpresaId)
    : IRequest<ClienteDto?>, ITenantRequest;

public class GetClienteByIdHandler : IRequestHandler<GetClienteByIdQuery, ClienteDto?>
{
    private readonly IClienteRepository _repo;
    public GetClienteByIdHandler(IClienteRepository repo) => _repo = repo;

    public async Task<ClienteDto?> Handle(GetClienteByIdQuery r, CancellationToken ct)
    {
        var c = await _repo.GetByIdAndEmpresaAsync(r.Id, r.EmpresaId, ct);
        if (c == null || c.IsDeleted) return null;

        return new ClienteDto(
            c.Id, c.Codigo, c.TipoPessoa, c.Nome, c.NomeFantasia,
            c.Documento, c.InscricaoEstadual, c.InscricaoMunicipal,
            c.Email, c.Telefone, c.Celular, c.Site,
            c.Status, c.Observacoes,
            c.Logradouro, c.Numero, c.Complemento, c.Bairro, c.Cidade, c.Estado, c.Cep,
            c.CreatedAt);
    }
}
