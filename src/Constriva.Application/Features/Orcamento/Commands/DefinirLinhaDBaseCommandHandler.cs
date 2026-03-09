using FluentValidation;
using MediatR;
using Constriva.Application.Common.Behaviors;
using Constriva.Application.Common.Interfaces;
using Constriva.Domain.Entities.Orcamento;
using Constriva.Domain.Enums;
using Constriva.Domain.Interfaces.Repositories;
using Constriva.Application.Features.Orcamento.DTOs;

namespace Constriva.Application.Features.Orcamento.Commands;

public record DefinirLinhaDBaseCommand(Guid Id, Guid EmpresaId) : IRequest<bool>, ITenantRequest;

public class DefinirLinhaDBaseHandler : IRequestHandler<DefinirLinhaDBaseCommand, bool>
{
    private readonly IOrcamentoRepository _repo;
    private readonly IUnitOfWork _uow;

    public DefinirLinhaDBaseHandler(IOrcamentoRepository repo, IUnitOfWork uow)
    {
        _repo = repo;
        _uow = uow;
    }

    public async Task<bool> Handle(DefinirLinhaDBaseCommand request, CancellationToken ct)
    {
        var orcamento = await _repo.GetByIdAsync(request.Id, request.EmpresaId, ct)
            ?? throw new KeyNotFoundException("Orçamento não encontrado.");

        if (orcamento.Status != StatusOrcamentoEnum.Aprovado)
            throw new InvalidOperationException("Apenas orçamentos aprovados podem ser definidos como linha de base.");

        var todos = await _repo.GetByObraAsync(orcamento.ObraId, request.EmpresaId, ct);
        foreach (var orc in todos.Where(o => o.ELinhaDBase && o.Id != request.Id))
        {
            orc.ELinhaDBase = false;
            _repo.Update(orc);
        }

        orcamento.ELinhaDBase = true;
        _repo.Update(orcamento);
        await _uow.SaveChangesAsync(ct);

        return true;
    }
}
