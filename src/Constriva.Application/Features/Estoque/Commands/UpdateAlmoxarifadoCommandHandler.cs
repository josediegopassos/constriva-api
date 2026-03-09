using FluentValidation;
using MediatR;
using Constriva.Application.Common.Behaviors;
using Constriva.Application.Common.Interfaces;
using Constriva.Domain.Entities.Estoque;
using Constriva.Domain.Enums;
using Constriva.Domain.Interfaces.Repositories;
using Constriva.Application.Features.Estoque.DTOs;

namespace Constriva.Application.Features.Estoque.Commands;

public record UpdateAlmoxarifadoCommand(Guid Id, Guid EmpresaId, string Nome, string? Responsavel)
    : IRequest<AlmoxarifadoDto>, ITenantRequest { public Guid TenantId => EmpresaId; }

public class UpdateAlmoxarifadoHandler : IRequestHandler<UpdateAlmoxarifadoCommand, AlmoxarifadoDto>
{
    private readonly IEstoqueRepository _repo;
    private readonly IUnitOfWork _uow;

    public UpdateAlmoxarifadoHandler(IEstoqueRepository repo, IUnitOfWork uow)
    {
        _repo = repo;
        _uow = uow;
    }

    public async Task<AlmoxarifadoDto> Handle(UpdateAlmoxarifadoCommand request, CancellationToken cancellationToken)
    {
        var almoxarifado = await _repo.GetAlmoxarifadoByIdAsync(request.Id, request.EmpresaId, cancellationToken)
            ?? throw new KeyNotFoundException($"Almoxarifado {request.Id} não encontrado.");

        almoxarifado.Nome = request.Nome;

        await _uow.SaveChangesAsync(cancellationToken);

        var codigo = $"ALM-{almoxarifado.Id.ToString()[..8].ToUpper()}";
        return new AlmoxarifadoDto(almoxarifado.Id, almoxarifado.Nome, codigo, almoxarifado.ObraId, almoxarifado.Principal);
    }
}
