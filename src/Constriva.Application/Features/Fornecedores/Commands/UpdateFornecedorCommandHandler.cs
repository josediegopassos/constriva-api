using MediatR;
using Constriva.Application.Common.Behaviors;
using Constriva.Domain.Entities.Common;
using Constriva.Domain.Interfaces.Repositories;
using Constriva.Application.Features.Fornecedores.DTOs;

namespace Constriva.Application.Features.Fornecedores.Commands;

public record UpdateFornecedorCommand(Guid Id, Guid EmpresaId, Guid UsuarioId, UpdateFornecedorDto Dto)
    : IRequest<Unit>, ITenantRequest;

public class UpdateFornecedorCommandHandler : IRequestHandler<UpdateFornecedorCommand, Unit>
{
    private readonly IFornecedorRepository _repo;
    private readonly IUnitOfWork _uow;

    public UpdateFornecedorCommandHandler(IFornecedorRepository repo, IUnitOfWork uow)
    { _repo = repo; _uow = uow; }

    public async Task<Unit> Handle(UpdateFornecedorCommand r, CancellationToken ct)
    {
        var fornecedor = await _repo.GetByIdComEnderecoAsync(r.Id, r.EmpresaId, ct)
            ?? throw new KeyNotFoundException($"Fornecedor {r.Id} não encontrado.");

        var dto = r.Dto;

        if (dto.Documento != null && dto.Documento != fornecedor.Documento)
        {
            var exists = await _repo.GetByDocumentoAsync(r.EmpresaId, dto.Documento, ct);
            if (exists != null && exists.Id != r.Id)
                throw new InvalidOperationException($"Já existe um fornecedor com o documento {dto.Documento}.");
        }

        if (dto.RazaoSocial != null)      fornecedor.RazaoSocial = dto.RazaoSocial;
        if (dto.TipoPessoa.HasValue)      fornecedor.TipoPessoaEnum = dto.TipoPessoa.Value;
        if (dto.NomeFantasia != null)     fornecedor.NomeFantasia = dto.NomeFantasia;
        if (dto.Documento != null)        fornecedor.Documento = dto.Documento;
        if (dto.InscricaoEstadual != null) fornecedor.InscricaoEstadual = dto.InscricaoEstadual;
        if (dto.Email != null)            fornecedor.Email = dto.Email;
        if (dto.Telefone != null)         fornecedor.Telefone = dto.Telefone;
        if (dto.Celular != null)          fornecedor.Celular = dto.Celular;
        if (dto.Site != null)             fornecedor.Site = dto.Site;
        if (dto.Contato != null)          fornecedor.Contato = dto.Contato;
        if (dto.Tipo.HasValue)            fornecedor.Tipo = dto.Tipo.Value;
        if (dto.Classificacao != null)    fornecedor.Classificacao = dto.Classificacao;
        if (dto.Prazo.HasValue)           fornecedor.Prazo = dto.Prazo.Value;
        if (dto.Observacoes != null)      fornecedor.Observacoes = dto.Observacoes;
        if (dto.BancoNome != null)        fornecedor.BancoNome = dto.BancoNome;
        if (dto.BancoAgencia != null)     fornecedor.BancoAgencia = dto.BancoAgencia;
        if (dto.BancoConta != null)       fornecedor.BancoConta = dto.BancoConta;
        if (dto.PixChave != null)         fornecedor.PixChave = dto.PixChave;
        bool hasAddressChange = dto.Logradouro != null || dto.Numero != null || dto.Complemento != null || dto.Bairro != null || dto.Cidade != null || dto.Estado != null || dto.Cep != null;
        if (hasAddressChange)
        {
            fornecedor.Endereco ??= new Endereco { EmpresaId = r.EmpresaId };
            if (dto.Logradouro != null)   fornecedor.Endereco.Logradouro = dto.Logradouro;
            if (dto.Numero != null)       fornecedor.Endereco.Numero = dto.Numero;
            if (dto.Complemento != null)  fornecedor.Endereco.Complemento = dto.Complemento;
            if (dto.Bairro != null)       fornecedor.Endereco.Bairro = dto.Bairro;
            if (dto.Cidade != null)       fornecedor.Endereco.Cidade = dto.Cidade;
            if (dto.Estado != null)       fornecedor.Endereco.Estado = dto.Estado;
            if (dto.Cep != null)          fornecedor.Endereco.Cep = dto.Cep;
        }

        fornecedor.Homologado = false;

        _repo.Update(fornecedor);
        await _uow.SaveChangesAsync(ct);
        return Unit.Value;
    }
}
