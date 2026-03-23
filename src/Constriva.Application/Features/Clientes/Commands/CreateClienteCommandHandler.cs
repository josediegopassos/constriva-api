using MediatR;
using Constriva.Application.Common.Behaviors;
using Constriva.Domain.Entities.Clientes;
using Constriva.Domain.Entities.Common;
using Constriva.Domain.Interfaces.Repositories;
using Constriva.Application.Features.Clientes.DTOs;

namespace Constriva.Application.Features.Clientes.Commands;

public record CreateClienteCommand(Guid EmpresaId, Guid UsuarioId, CreateClienteDto Dto)
    : IRequest<ClienteResumoDto>, ITenantRequest;

public class CreateClienteCommandHandler : IRequestHandler<CreateClienteCommand, ClienteResumoDto>
{
    private readonly IClienteRepository _repo;
    private readonly IUnitOfWork _uow;

    public CreateClienteCommandHandler(IClienteRepository repo, IUnitOfWork uow)
    { _repo = repo; _uow = uow; }

    public async Task<ClienteResumoDto> Handle(CreateClienteCommand r, CancellationToken ct)
    {
        if (r.Dto.Documento != null)
        {
            var exists = await _repo.DocumentoExistsAsync(r.Dto.Documento, r.EmpresaId, null, ct);
            if (exists) throw new InvalidOperationException($"Já existe um cliente com o documento {r.Dto.Documento}.");
        }

        var count = await _repo.GetCountByEmpresaAsync(r.EmpresaId, ct);
        var cliente = new Cliente
        {
            EmpresaId = r.EmpresaId,
            Codigo = $"CLI-{(count + 1):D4}",
            TipoPessoa = r.Dto.TipoPessoa,
            Nome = r.Dto.Nome,
            NomeFantasia = r.Dto.NomeFantasia,
            Documento = r.Dto.Documento,
            InscricaoEstadual = r.Dto.InscricaoEstadual,
            InscricaoMunicipal = r.Dto.InscricaoMunicipal,
            Email = r.Dto.Email,
            Telefone = r.Dto.Telefone,
            Celular = r.Dto.Celular,
            Site = r.Dto.Site,
            Observacoes = r.Dto.Observacoes,
            Endereco = new Endereco
            {
                EmpresaId = r.EmpresaId,
                Logradouro = r.Dto.Logradouro,
                Numero = r.Dto.Numero,
                Complemento = r.Dto.Complemento,
                Bairro = r.Dto.Bairro,
                Cidade = r.Dto.Cidade,
                Estado = r.Dto.Estado,
                Cep = r.Dto.Cep
            },
        };

        await _repo.AddAsync(cliente, ct);
        await _uow.SaveChangesAsync(ct);

        return ToResumo(cliente);
    }

    internal static ClienteResumoDto ToResumo(Cliente c) => new(
        c.Id, c.Codigo, c.TipoPessoa, c.Nome, c.NomeFantasia,
        c.Documento, c.Email, c.Telefone, c.Status, c.Endereco?.Cidade, c.Endereco?.Estado);
}
