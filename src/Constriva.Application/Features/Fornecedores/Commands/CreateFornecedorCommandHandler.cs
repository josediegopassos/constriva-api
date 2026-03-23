using MediatR;
using Constriva.Application.Common.Behaviors;
using Constriva.Domain.Entities.Common;
using Constriva.Domain.Entities.Fornecedores;
using Constriva.Domain.Interfaces.Repositories;
using Constriva.Application.Features.Fornecedores.DTOs;

namespace Constriva.Application.Features.Fornecedores.Commands;

public record CreateFornecedorCommand(Guid EmpresaId, Guid UsuarioId, CreateFornecedorDto Dto)
    : IRequest<FornecedorResumoDto>, ITenantRequest;

public class CreateFornecedorCommandHandler : IRequestHandler<CreateFornecedorCommand, FornecedorResumoDto>
{
    private readonly IFornecedorRepository _repo;
    private readonly IUnitOfWork _uow;

    public CreateFornecedorCommandHandler(IFornecedorRepository repo, IUnitOfWork uow)
    { _repo = repo; _uow = uow; }

    public async Task<FornecedorResumoDto> Handle(CreateFornecedorCommand r, CancellationToken ct)
    {
        var existing = await _repo.GetByDocumentoAsync(r.EmpresaId, r.Dto.Documento, ct);
        if (existing != null)
            throw new InvalidOperationException($"Já existe um fornecedor com o documento {r.Dto.Documento}.");

        var count = await _repo.CountAsync(f => f.EmpresaId == r.EmpresaId, ct);
        var dto = r.Dto;

        var fornecedor = new Fornecedor
        {
            EmpresaId = r.EmpresaId,
            Codigo = $"FOR-{(count + 1):D4}",
            TipoPessoaEnum = dto.TipoPessoa,
            RazaoSocial = dto.RazaoSocial,
            NomeFantasia = dto.NomeFantasia,
            Documento = dto.Documento,
            InscricaoEstadual = dto.InscricaoEstadual,
            Email = dto.Email,
            Telefone = dto.Telefone,
            Celular = dto.Celular,
            Site = dto.Site,
            Contato = dto.Contato,
            Tipo = dto.Tipo,
            Classificacao = dto.Classificacao,
            Prazo = dto.Prazo,
            Observacoes = dto.Observacoes,
            BancoNome = dto.BancoNome,
            BancoAgencia = dto.BancoAgencia,
            BancoConta = dto.BancoConta,
            PixChave = dto.PixChave,
            Endereco = new Endereco
            {
                EmpresaId = r.EmpresaId,
                Logradouro = dto.Logradouro,
                Numero = dto.Numero,
                Complemento = dto.Complemento,
                Bairro = dto.Bairro,
                Cidade = dto.Cidade,
                Estado = dto.Estado,
                Cep = dto.Cep
            }
        };

        await _repo.AddAsync(fornecedor, ct);
        await _uow.SaveChangesAsync(ct);

        return ToResumo(fornecedor);
    }

    internal static FornecedorResumoDto ToResumo(Fornecedor f) => new(
        f.Id, f.Codigo, f.TipoPessoaEnum, f.RazaoSocial, f.NomeFantasia,
        f.Documento, f.Email, f.Telefone, f.Tipo, f.Ativo, f.Homologado,
        f.Endereco?.Cidade, f.Endereco?.Estado);

    internal static FornecedorDto ToDto(Fornecedor f) => new(
        f.Id, f.Codigo, f.TipoPessoaEnum, f.RazaoSocial, f.NomeFantasia,
        f.Documento, f.InscricaoEstadual, f.Email, f.Telefone, f.Celular,
        f.Site, f.Contato, f.Tipo, f.Ativo, f.Homologado,
        f.Classificacao, f.Prazo, f.Observacoes,
        f.BancoNome, f.BancoAgencia, f.BancoConta, f.PixChave,
        f.Endereco?.Logradouro, f.Endereco?.Numero, f.Endereco?.Complemento, f.Endereco?.Bairro, f.Endereco?.Cidade, f.Endereco?.Estado, f.Endereco?.Cep,
        f.CreatedAt);
}
