using FluentValidation;
using MediatR;
using Constriva.Application.Common.Behaviors;
using Constriva.Application.Common.Interfaces;
using Constriva.Domain.Entities.GED;
using Constriva.Domain.Enums;
using Constriva.Domain.Interfaces.Repositories;
using Microsoft.AspNetCore.Http;
using Constriva.Application.Features.GED.DTOs;

namespace Constriva.Application.Features.GED.Commands;

public record CreatePastaCommand(Guid EmpresaId, string Nome, Guid? ObraId, Guid? PastaPaiId)
    : IRequest<PastaDto>, ITenantRequest { public Guid TenantId => EmpresaId; }

public class CreatePastaHandler : IRequestHandler<CreatePastaCommand, PastaDto>
{
    private readonly IGEDRepository _repo;
    private readonly IUnitOfWork _uow;

    public CreatePastaHandler(IGEDRepository repo, IUnitOfWork uow)
    {
        _repo = repo;
        _uow = uow;
    }

    public async Task<PastaDto> Handle(CreatePastaCommand request, CancellationToken cancellationToken)
    {
        var pasta = new PastaDocumento
        {
            EmpresaId = request.EmpresaId,
            Nome = request.Nome,
            ObraId = request.ObraId,
            PastaPaiId = request.PastaPaiId,
            Ativo = true
        };

        await _repo.AddPastaAsync(pasta, cancellationToken);
        await _uow.SaveChangesAsync(cancellationToken);

        return new PastaDto(pasta.Id, pasta.Nome, pasta.Descricao, pasta.ObraId, pasta.PastaPaiId, pasta.AcessoPublico, 0);
    }
}
