using FluentValidation;
using MediatR;
using Constriva.Application.Common.Behaviors;
using Constriva.Domain.Entities.Contratos;
using Constriva.Domain.Enums;
using Constriva.Domain.Interfaces.Repositories;
using Constriva.Application.Features.Contratos.DTOs;

namespace Constriva.Application.Features.Contratos.Commands;

public record DeleteMedicaoCommand(Guid Id, Guid EmpresaId) : IRequest<Unit>, ITenantRequest
    { public Guid TenantId => EmpresaId; }

public class DeleteMedicaoHandler : IRequestHandler<DeleteMedicaoCommand, Unit>
{
    private readonly IContratoRepository _repo;
    private readonly IUnitOfWork _uow;

    public DeleteMedicaoHandler(IContratoRepository repo, IUnitOfWork uow)
    {
        _repo = repo;
        _uow = uow;
    }

    public async Task<Unit> Handle(DeleteMedicaoCommand request, CancellationToken cancellationToken)
    {
        var medicao = await _repo.GetMedicaoByIdAsync(request.Id, request.EmpresaId, cancellationToken)
            ?? throw new KeyNotFoundException($"Medição {request.Id} não encontrada.");

        if (medicao.Status != StatusMedicaoEnum.Rascunho)
            throw new InvalidOperationException(
                $"Medição no status '{medicao.Status}' não pode ser excluída. Apenas rascunhos podem ser removidos.");

        medicao.IsDeleted = true;
        await _uow.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}
