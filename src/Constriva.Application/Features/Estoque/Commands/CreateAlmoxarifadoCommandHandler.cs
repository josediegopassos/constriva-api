using FluentValidation;
using MediatR;
using Constriva.Application.Common.Behaviors;
using Constriva.Application.Common.Interfaces;
using Constriva.Domain.Entities.Estoque;
using Constriva.Domain.Enums;
using Constriva.Domain.Interfaces.Repositories;
using Constriva.Application.Features.Estoque.DTOs;

namespace Constriva.Application.Features.Estoque.Commands;

public record CreateAlmoxarifadoCommand(Guid EmpresaId, string Nome, Guid? ObraId, string? Responsavel)
    : IRequest<AlmoxarifadoDto>, ITenantRequest { public Guid TenantId => EmpresaId; }

public class CreateAlmoxarifadoHandler : IRequestHandler<CreateAlmoxarifadoCommand, AlmoxarifadoDto>
{
    private readonly IEstoqueRepository _repo;
    private readonly IUnitOfWork _uow;

    public CreateAlmoxarifadoHandler(IEstoqueRepository repo, IUnitOfWork uow)
    {
        _repo = repo;
        _uow = uow;
    }

    public async Task<AlmoxarifadoDto> Handle(CreateAlmoxarifadoCommand request, CancellationToken cancellationToken)
    {
        var almoxarifado = new Almoxarifado
        {
            EmpresaId = request.EmpresaId,
            Nome = request.Nome,
            ObraId = request.ObraId,
            Ativo = true,
            Principal = false
        };

        await _repo.AddAlmoxarifadoAsync(almoxarifado, cancellationToken);
        await _uow.SaveChangesAsync(cancellationToken);

        var codigo = $"ALM-{almoxarifado.Id.ToString()[..8].ToUpper()}";
        return new AlmoxarifadoDto(almoxarifado.Id, almoxarifado.Nome, codigo, almoxarifado.ObraId, almoxarifado.Principal);
    }
}
