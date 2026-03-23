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

public record CreateFornecedorCommand(Guid EmpresaId, CreateFornecedorDto Dto)
    : IRequest<FornecedorDto>, ITenantRequest;

public class CreateFornecedorHandler : IRequestHandler<CreateFornecedorCommand, FornecedorDto>
{
    private readonly IFornecedorRepository _repo;
    private readonly IUnitOfWork _uow;

    public CreateFornecedorHandler(IFornecedorRepository repo, IUnitOfWork uow)
    {
        _repo = repo;
        _uow = uow;
    }

    public async Task<FornecedorDto> Handle(CreateFornecedorCommand request, CancellationToken cancellationToken)
    {
        var dto = request.Dto;
        var documento = dto.Cnpj ?? dto.Cpf ?? throw new ArgumentException("CNPJ ou CPF é obrigatório.");

        var fornecedor = new Fornecedor
        {
            EmpresaId = request.EmpresaId,
            Codigo = $"FOR-{Guid.NewGuid().ToString()[..8].ToUpper()}",
            RazaoSocial = dto.RazaoSocial,
            NomeFantasia = dto.NomeFantasia ?? dto.RazaoSocial,
            Documento = documento,
            TipoPessoaEnum = dto.Cnpj != null ? TipoPessoaEnum.PessoaJuridica : TipoPessoaEnum.PessoaFisica,
            Tipo = dto.Tipo,
            Email = dto.Email ?? "",
            Telefone = dto.Telefone,
            Endereco = new Endereco
            {
                EmpresaId = request.EmpresaId,
                Cidade = dto.Cidade,
                Estado = dto.Estado
            },
            Ativo = true
        };

        await _repo.AddAsync(fornecedor, cancellationToken);
        await _uow.SaveChangesAsync(cancellationToken);

        return new FornecedorDto(
            fornecedor.Id, fornecedor.RazaoSocial, fornecedor.NomeFantasia,
            dto.Cnpj, dto.Cpf, fornecedor.Tipo,
            fornecedor.Telefone, fornecedor.Email, fornecedor.Endereco?.Cidade, fornecedor.Ativo);
    }
}
