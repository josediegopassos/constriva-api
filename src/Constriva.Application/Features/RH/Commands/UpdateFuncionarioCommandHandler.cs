using FluentValidation;
using MediatR;
using Constriva.Application.Common.Behaviors;
using Constriva.Domain.Entities.RH;
using Constriva.Domain.Enums;
using Constriva.Domain.Interfaces.Repositories;
using Constriva.Application.Features.RH.DTOs;

namespace Constriva.Application.Features.RH.Commands;

public record UpdateFuncionarioCommand(Guid Id, Guid EmpresaId, string Nome, string Cargo,
    string? Departamento, decimal SalarioBase, string? Telefone, string? Email,
    bool Ativo, Guid? ObraId)
    : IRequest<FuncionarioDto>, ITenantRequest { public Guid TenantId => EmpresaId; }

public class UpdateFuncionarioCommandHandler : IRequestHandler<UpdateFuncionarioCommand, FuncionarioDto>
{
    private readonly IRHRepository _repo;
    private readonly IUnitOfWork _uow;

    public UpdateFuncionarioCommandHandler(IRHRepository repo, IUnitOfWork uow)
    {
        _repo = repo;
        _uow = uow;
    }

    public async Task<FuncionarioDto> Handle(UpdateFuncionarioCommand request, CancellationToken cancellationToken)
    {
        var funcionario = await _repo.GetFuncionarioByIdAsync(request.Id, request.EmpresaId, cancellationToken)
            ?? throw new KeyNotFoundException($"Funcionário {request.Id} não encontrado.");

        funcionario.Nome = request.Nome;
        funcionario.SalarioBase = request.SalarioBase;
        if (request.Telefone != null) funcionario.Telefone = request.Telefone;
        if (request.Email != null) funcionario.Email = request.Email;
        if (request.ObraId.HasValue) funcionario.ObraAtualId = request.ObraId;

        await _uow.SaveChangesAsync(cancellationToken);

        return new FuncionarioDto(
            funcionario.Id, funcionario.Nome, funcionario.Cpf, funcionario.Email,
            funcionario.Telefone, funcionario.CargoId, null,
            funcionario.DepartamentoId, null,
            funcionario.DataAdmissao, funcionario.SalarioBase, funcionario.Status);
    }
}
