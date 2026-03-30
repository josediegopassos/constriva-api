using MediatR;
using Constriva.Application.Common.Behaviors;
using Constriva.Domain.Entities.Common;
using Constriva.Domain.Interfaces.Repositories;
using Constriva.Application.Features.Clientes.DTOs;

namespace Constriva.Application.Features.Clientes.Commands;

public record UpdateClienteCommand(Guid Id, Guid EmpresaId, Guid UsuarioId, UpdateClienteDto Dto)
    : IRequest<Unit>, ITenantRequest;

public class UpdateClienteCommandHandler : IRequestHandler<UpdateClienteCommand, Unit>
{
    private readonly IClienteRepository _repo;
    private readonly IUnitOfWork _uow;

    public UpdateClienteCommandHandler(IClienteRepository repo, IUnitOfWork uow)
    { _repo = repo; _uow = uow; }

    public async Task<Unit> Handle(UpdateClienteCommand r, CancellationToken ct)
    {
        var cliente = await _repo.GetByIdComEnderecoAsync(r.Id, r.EmpresaId, ct)
            ?? throw new KeyNotFoundException($"Cliente {r.Id} não encontrado.");

        var dto = r.Dto;

        if (dto.Documento != null && dto.Documento != cliente.Documento)
        {
            var exists = await _repo.DocumentoExistsAsync(dto.Documento, r.EmpresaId, r.Id, ct);
            if (exists) throw new InvalidOperationException($"Já existe um cliente com o documento {dto.Documento}.");
        }

        if (dto.Nome != null)              cliente.Nome = dto.Nome;
        if (dto.TipoPessoa.HasValue)       cliente.TipoPessoa = dto.TipoPessoa.Value;
        if (dto.NomeFantasia != null)      cliente.NomeFantasia = dto.NomeFantasia;
        if (dto.Documento != null)         cliente.Documento = dto.Documento;
        if (dto.InscricaoEstadual != null) cliente.InscricaoEstadual = dto.InscricaoEstadual;
        if (dto.InscricaoMunicipal != null) cliente.InscricaoMunicipal = dto.InscricaoMunicipal;
        if (dto.Email != null)             cliente.Email = dto.Email;
        if (dto.Telefone != null)          cliente.Telefone = dto.Telefone;
        if (dto.Celular != null)           cliente.Celular = dto.Celular;
        if (dto.Site != null)              cliente.Site = dto.Site;
        if (dto.Status.HasValue)           cliente.Status = dto.Status.Value;
        if (dto.Observacoes != null)       cliente.Observacoes = dto.Observacoes;

        if (dto.Endereco != null)
        {
            if (cliente.Endereco == null)
            {
                var endereco = new Endereco { EmpresaId = r.EmpresaId };
                await _repo.AddEnderecoAsync(endereco, ct);
                cliente.EnderecoId = endereco.Id;
                cliente.Endereco = endereco;
            }
            if (dto.Endereco.Logradouro != null)    cliente.Endereco.Logradouro = dto.Endereco.Logradouro;
            if (dto.Endereco.Numero != null)        cliente.Endereco.Numero = dto.Endereco.Numero;
            if (dto.Endereco.Complemento != null)   cliente.Endereco.Complemento = dto.Endereco.Complemento;
            if (dto.Endereco.Bairro != null)        cliente.Endereco.Bairro = dto.Endereco.Bairro;
            if (dto.Endereco.Cidade != null)        cliente.Endereco.Cidade = dto.Endereco.Cidade;
            if (dto.Endereco.Estado != null)        cliente.Endereco.Estado = dto.Endereco.Estado;
            if (dto.Endereco.Cep != null)           cliente.Endereco.Cep = dto.Endereco.Cep;
        }

        cliente.UpdatedBy = r.UsuarioId;
        cliente.UpdatedAt = DateTime.UtcNow;

        await _uow.SaveChangesAsync(ct);
        return Unit.Value;
    }
}
