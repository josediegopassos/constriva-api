using MediatR;
using Constriva.Application.Common.Behaviors;
using Constriva.Domain.Entities.RH;
using Constriva.Domain.Enums;
using Constriva.Domain.Interfaces.Repositories;
using Constriva.Application.Features.RH.DTOs;

namespace Constriva.Application.Features.RH.Commands;

public record CreateFuncionarioCommand(Guid EmpresaId, CreateFuncionarioDto Dto)
    : IRequest<FuncionarioDto>, ITenantRequest;

public class CreateFuncionarioCommandHandler : IRequestHandler<CreateFuncionarioCommand, FuncionarioDto>
{
    private readonly IRHRepository _repo;
    private readonly IUnitOfWork _uow;

    public CreateFuncionarioCommandHandler(IRHRepository repo, IUnitOfWork uow)
    {
        _repo = repo;
        _uow = uow;
    }

    public async Task<FuncionarioDto> Handle(CreateFuncionarioCommand request, CancellationToken cancellationToken)
    {
        var dto = request.Dto;
        var funcionario = new Funcionario
        {
            EmpresaId = request.EmpresaId,
            Nome = dto.Nome,
            Cpf = dto.Cpf,
            Email = dto.Email,
            Telefone = dto.Telefone,
            CargoId = dto.CargoId,
            DepartamentoId = dto.DepartamentoId,
            DataAdmissao = dto.DataAdmissao,
            SalarioBase = dto.SalarioBase,
            Status = dto.Status
        };

        await _repo.AddFuncionarioAsync(funcionario, cancellationToken);
        await _uow.SaveChangesAsync(cancellationToken);

        return new FuncionarioDto(
            funcionario.Id, funcionario.Nome, funcionario.Cpf, funcionario.Email,
            funcionario.Telefone, funcionario.CargoId, null,
            funcionario.DepartamentoId, null,
            funcionario.DataAdmissao, funcionario.SalarioBase, funcionario.Status);
    }
}
