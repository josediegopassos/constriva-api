using FluentValidation;
using MediatR;
using Constriva.Application.Common.Interfaces;
using Constriva.Domain.Entities.Tenant;
using Constriva.Domain.Enums;
using Constriva.Domain.Interfaces.Repositories;
using Constriva.Application.Features.Empresas.DTOs;

namespace Constriva.Application.Features.Empresas.Commands;

public record UpdateEmpresaCommand(Guid Id, UpdateEmpresaDto Dto) : IRequest<EmpresaDto>;

public class UpdateEmpresaHandler : IRequestHandler<UpdateEmpresaCommand, EmpresaDto>
{
    private readonly IEmpresaRepository _repo;
    private readonly IUnitOfWork _uow;

    public UpdateEmpresaHandler(IEmpresaRepository repo, IUnitOfWork uow)
    {
        _repo = repo;
        _uow = uow;
    }

    public async Task<EmpresaDto> Handle(UpdateEmpresaCommand request, CancellationToken cancellationToken)
    {
        var empresa = await _repo.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new KeyNotFoundException($"Empresa {request.Id} não encontrada.");

        var dto = request.Dto;
        empresa.RazaoSocial = dto.RazaoSocial;
        empresa.NomeFantasia = dto.NomeFantasia;
        empresa.Email = dto.Email;
        empresa.Telefone = dto.Telefone;
        empresa.Logradouro = dto.Logradouro;
        empresa.Numero = dto.Numero;
        empresa.Complemento = dto.Complemento;
        empresa.Bairro = dto.Bairro;
        empresa.Cidade = dto.Cidade;
        empresa.Estado = dto.Estado;
        empresa.Cep = dto.Cep;

        _repo.Update(empresa);
        await _uow.SaveChangesAsync(cancellationToken);

        return new EmpresaDto(
            empresa.Id, empresa.RazaoSocial, empresa.NomeFantasia, empresa.Cnpj,
            empresa.Email, empresa.Telefone, empresa.LogoUrl,
            empresa.Status, empresa.Plano, empresa.DataVencimento,
            empresa.MaxUsuarios, empresa.MaxObras, empresa.Status == StatusEmpresaEnum.Ativa,
            empresa.Cidade, empresa.Estado,
            new ModulosConfigDto(empresa.ModuloObras, empresa.ModuloEstoque, empresa.ModuloCronograma,
                empresa.ModuloOrcamento, empresa.ModuloCompras, empresa.ModuloQualidade,
                empresa.ModuloContratos, empresa.ModuloRH, empresa.ModuloFinanceiro,
                empresa.ModuloSST, empresa.ModuloGED, empresa.ModuloRelatorios),
            0, 0, empresa.CreatedAt);
    }
}
