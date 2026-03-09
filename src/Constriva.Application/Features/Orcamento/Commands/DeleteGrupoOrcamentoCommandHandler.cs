using FluentValidation;
using MediatR;
using Constriva.Application.Common.Behaviors;
using Constriva.Application.Common.Interfaces;
using Constriva.Domain.Entities.Orcamento;
using Constriva.Domain.Enums;
using Constriva.Domain.Interfaces.Repositories;
using Constriva.Application.Features.Orcamento.DTOs;

namespace Constriva.Application.Features.Orcamento.Commands;

public record DeleteGrupoOrcamentoCommand(Guid Id, Guid EmpresaId) : IRequest<bool>, ITenantRequest;

public class DeleteGrupoOrcamentoHandler : IRequestHandler<DeleteGrupoOrcamentoCommand, bool>
{
    private readonly IGrupoOrcamentoRepository _grupoRepo;
    private readonly IOrcamentoRepository _orcRepo;
    private readonly IUnitOfWork _uow;

    public DeleteGrupoOrcamentoHandler(
        IGrupoOrcamentoRepository grupoRepo,
        IOrcamentoRepository orcRepo,
        IUnitOfWork uow)
    {
        _grupoRepo = grupoRepo;
        _orcRepo = orcRepo;
        _uow = uow;
    }

    public async Task<bool> Handle(DeleteGrupoOrcamentoCommand request, CancellationToken ct)
    {
        var grupo = await _grupoRepo.GetByIdAsync(request.Id, request.EmpresaId, ct)
            ?? throw new KeyNotFoundException("Grupo não encontrado.");

        var orcamento = await _orcRepo.GetByIdAsync(grupo.OrcamentoId, request.EmpresaId, ct);

        if (orcamento?.Status == StatusOrcamentoEnum.Aprovado)
            throw new InvalidOperationException("Não é possível remover grupos de um orçamento aprovado.");

        _grupoRepo.Remove(grupo);

        if (orcamento != null)
        {
            orcamento.ValorCustoDirecto = Math.Max(0, orcamento.ValorCustoDirecto - grupo.ValorTotal);
            orcamento.ValorBDI = orcamento.ValorCustoDirecto * orcamento.BDI / 100;
            orcamento.ValorTotal = orcamento.ValorCustoDirecto + orcamento.ValorBDI;
            _orcRepo.Update(orcamento);
        }

        await _uow.SaveChangesAsync(ct);
        return true;
    }
}
