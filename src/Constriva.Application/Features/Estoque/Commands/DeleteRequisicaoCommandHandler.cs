using FluentValidation;
using MediatR;
using Constriva.Application.Common.Behaviors;
using Constriva.Application.Common.Interfaces;
using Constriva.Domain.Entities.Estoque;
using Constriva.Domain.Enums;
using Constriva.Domain.Interfaces.Repositories;
using Constriva.Application.Features.Estoque.DTOs;

namespace Constriva.Application.Features.Estoque.Commands;

public record DeleteRequisicaoCommand(Guid Id, Guid EmpresaId) : IRequest<Unit>, ITenantRequest
    { public Guid TenantId => EmpresaId; }

public class DeleteRequisicaoHandler : IRequestHandler<DeleteRequisicaoCommand, Unit>
{
    private readonly IEstoqueRepository _repo;
    private readonly IUnitOfWork _uow;

    public DeleteRequisicaoHandler(IEstoqueRepository repo, IUnitOfWork uow)
    {
        _repo = repo;
        _uow = uow;
    }

    public async Task<Unit> Handle(DeleteRequisicaoCommand request, CancellationToken cancellationToken)
    {
        var requisicao = await _repo.GetRequisicaoByIdAsync(request.Id, request.EmpresaId, cancellationToken)
            ?? throw new KeyNotFoundException($"Requisição {request.Id} não encontrada.");

        if (requisicao.Status != StatusRequisicaoEnum.Pendente)
            throw new InvalidOperationException(
                $"Requisição no status '{requisicao.Status}' não pode ser excluída. Apenas requisições pendentes podem ser removidas.");

        requisicao.IsDeleted = true;
        await _uow.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}
