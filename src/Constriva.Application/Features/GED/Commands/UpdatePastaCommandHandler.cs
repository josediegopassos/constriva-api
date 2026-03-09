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

public record UpdatePastaCommand(Guid Id, Guid EmpresaId, string Nome)
    : IRequest<PastaDto>, ITenantRequest { public Guid TenantId => EmpresaId; }

public class UpdatePastaHandler : IRequestHandler<UpdatePastaCommand, PastaDto>
{
    private readonly IGEDRepository _repo;
    private readonly IUnitOfWork _uow;

    public UpdatePastaHandler(IGEDRepository repo, IUnitOfWork uow)
    {
        _repo = repo;
        _uow = uow;
    }

    public async Task<PastaDto> Handle(UpdatePastaCommand request, CancellationToken cancellationToken)
    {
        var pasta = await _repo.GetPastaAsync(request.Id, request.EmpresaId, cancellationToken)
            ?? throw new KeyNotFoundException($"Pasta {request.Id} não encontrada.");

        pasta.Nome = request.Nome;
        await _uow.SaveChangesAsync(cancellationToken);

        return new PastaDto(pasta.Id, pasta.Nome, pasta.Descricao, pasta.ObraId, pasta.PastaPaiId, pasta.AcessoPublico, 0);
    }
}
