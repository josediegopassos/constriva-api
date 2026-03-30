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
        var c = await _repo.GetByIdComEnderecoAsync(r.Id, r.EmpresaId, ct);
        if (c == null || c.IsDeleted) return null;

        var endereco = c.Endereco != null
            ? new EnderecoDto(c.Endereco.Logradouro, c.Endereco.Numero, c.Endereco.Complemento, c.Endereco.Bairro, c.Endereco.Cidade, c.Endereco.Estado, c.Endereco.Cep)
            : null;

        return new ClienteDto(
            c.Id, c.Codigo, c.TipoPessoa, c.Nome, c.NomeFantasia,
            c.Documento, c.InscricaoEstadual, c.InscricaoMunicipal,
            c.Email, c.Telefone, c.Celular, c.Site,
            c.Status, c.Observacoes,
            endereco,
            c.CreatedAt);
    }
}
