using FluentValidation;
using MediatR;
using Constriva.Application.Common.Behaviors;
using Constriva.Application.Common.Interfaces;
using Constriva.Domain.Entities.Common;
using Constriva.Domain.Entities.Compras;
using Constriva.Domain.Entities.Fornecedores;
using Constriva.Domain.Enums;
using Constriva.Domain.Interfaces.Repositories;
using Constriva.Application.Features.Compras.DTOs;

namespace Constriva.Application.Features.Compras.Commands;

public record UpdateFornecedorCommand(Guid Id, Guid EmpresaId, string RazaoSocial,
    string? NomeFantasia, string? CNPJ, string? Email, string? Telefone,
    string? Endereco, string Tipo)
    : IRequest<FornecedorDto>, ITenantRequest { public Guid TenantId => EmpresaId; }

public class UpdateFornecedorHandler : IRequestHandler<UpdateFornecedorCommand, FornecedorDto>
{
    private readonly IFornecedorRepository _repo;
    private readonly IUnitOfWork _uow;

    public UpdateFornecedorHandler(IFornecedorRepository repo, IUnitOfWork uow)
    {
        _repo = repo;
        _uow = uow;
    }

    public async Task<FornecedorDto> Handle(UpdateFornecedorCommand request, CancellationToken cancellationToken)
    {
        var fornecedor = await _repo.GetByIdComEnderecoAsync(request.Id, request.EmpresaId, cancellationToken)
            ?? throw new KeyNotFoundException($"Fornecedor {request.Id} não encontrado.");

        fornecedor.RazaoSocial = request.RazaoSocial;
        if (request.NomeFantasia != null) fornecedor.NomeFantasia = request.NomeFantasia;
        if (request.CNPJ != null) fornecedor.Documento = request.CNPJ;
        if (request.Email != null) fornecedor.Email = request.Email;
        if (request.Telefone != null) fornecedor.Telefone = request.Telefone;
        if (request.Endereco != null)
        {
            fornecedor.Endereco ??= new Endereco { EmpresaId = request.EmpresaId };
            fornecedor.Endereco.Logradouro = request.Endereco;
        }
        if (Enum.TryParse<TipoFornecedorEnum>(request.Tipo, out var tipo)) fornecedor.Tipo = tipo;

        _repo.Update(fornecedor);
        await _uow.SaveChangesAsync(cancellationToken);

        var cnpj = fornecedor.TipoPessoaEnum == TipoPessoaEnum.PessoaJuridica ? fornecedor.Documento : null;
        var cpf = fornecedor.TipoPessoaEnum == TipoPessoaEnum.PessoaFisica ? fornecedor.Documento : null;
        return new FornecedorDto(
            fornecedor.Id, fornecedor.RazaoSocial, fornecedor.NomeFantasia ?? fornecedor.RazaoSocial,
            cnpj, cpf, fornecedor.Tipo,
            fornecedor.Telefone, fornecedor.Email, fornecedor.Endereco?.Cidade, fornecedor.Ativo);
    }
}
