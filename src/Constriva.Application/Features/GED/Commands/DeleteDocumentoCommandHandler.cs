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

public record DeleteDocumentoCommand(Guid Id, Guid EmpresaId) : IRequest<Unit>, ITenantRequest
    { public Guid TenantId => EmpresaId; }

public class DeleteDocumentoHandler : IRequestHandler<DeleteDocumentoCommand, Unit>
{
    private readonly IGEDRepository _repo;
    private readonly IUnitOfWork _uow;

    public DeleteDocumentoHandler(IGEDRepository repo, IUnitOfWork uow)
    {
        _repo = repo;
        _uow = uow;
    }

    public async Task<Unit> Handle(DeleteDocumentoCommand request, CancellationToken cancellationToken)
    {
        var doc = await _repo.GetDocumentoByIdAsync(request.Id, request.EmpresaId, cancellationToken)
            ?? throw new KeyNotFoundException($"Documento {request.Id} não encontrado.");

        doc.IsDeleted = true;
        await _uow.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}
