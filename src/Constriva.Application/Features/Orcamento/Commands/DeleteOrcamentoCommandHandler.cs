using FluentValidation;
using MediatR;
using Constriva.Application.Common.Behaviors;
using Constriva.Application.Common.Interfaces;
using Constriva.Domain.Entities.Orcamento;
using Constriva.Domain.Enums;
using Constriva.Domain.Interfaces.Repositories;
using Constriva.Application.Features.Orcamento.DTOs;

namespace Constriva.Application.Features.Orcamento.Commands;

public record DeleteOrcamentoCommand(Guid Id, Guid EmpresaId) : IRequest<bool>, ITenantRequest;

public class DeleteOrcamentoHandler : IRequestHandler<DeleteOrcamentoCommand, bool>
{
    private readonly IOrcamentoRepository _repo;
    private readonly IUnitOfWork _uow;

    public DeleteOrcamentoHandler(IOrcamentoRepository repo, IUnitOfWork uow)
    {
        _repo = repo;
        _uow = uow;
    }

    public async Task<bool> Handle(DeleteOrcamentoCommand request, CancellationToken ct)
    {
        var orcamento = await _repo.GetByIdAsync(request.Id, request.EmpresaId, ct)
            ?? throw new KeyNotFoundException("Orçamento não encontrado.");

        if (orcamento.Status == StatusOrcamentoEnum.Aprovado)
            throw new InvalidOperationException("Não é possível excluir um orçamento aprovado.");

        orcamento.IsDeleted = true;
        _repo.Update(orcamento);
        await _uow.SaveChangesAsync(ct);
        return true;
    }
}
