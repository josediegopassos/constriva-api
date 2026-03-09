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

public record DeletePastaCommand(Guid Id, Guid EmpresaId) : IRequest<Unit>, ITenantRequest
    { public Guid TenantId => EmpresaId; }

public class DeletePastaHandler : IRequestHandler<DeletePastaCommand, Unit>
{
    private readonly IGEDRepository _repo;
    private readonly IUnitOfWork _uow;

    public DeletePastaHandler(IGEDRepository repo, IUnitOfWork uow)
    {
        _repo = repo;
        _uow = uow;
    }

    public async Task<Unit> Handle(DeletePastaCommand request, CancellationToken cancellationToken)
    {
        var pasta = await _repo.GetPastaAsync(request.Id, request.EmpresaId, cancellationToken)
            ?? throw new KeyNotFoundException($"Pasta {request.Id} não encontrada.");

        // Block deletion if there are active documents
        var (_, totalDocs) = await _repo.GetDocumentosPagedAsync(
            request.EmpresaId, request.Id, null, null, 1, 1, cancellationToken);
        if (totalDocs > 0)
            throw new InvalidOperationException(
                $"A pasta possui {totalDocs} documento(s) e não pode ser excluída. Mova ou exclua os documentos primeiro.");

        pasta.Ativo = false;
        pasta.IsDeleted = true;
        await _uow.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}
