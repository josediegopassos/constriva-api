using FluentValidation;
using MediatR;
using Constriva.Application.Common.Behaviors;
using Constriva.Application.Common.Interfaces;
using Constriva.Domain.Entities.Estoque;
using Constriva.Domain.Enums;
using Constriva.Domain.Interfaces.Repositories;
using Constriva.Application.Features.Estoque.DTOs;

namespace Constriva.Application.Features.Estoque.Commands;

public record UpdateRequisicaoCommand(Guid Id, Guid EmpresaId, string Descricao,
    DateTime DataNecessidade, string? Observacoes)
    : IRequest<RequisicaoDto>, ITenantRequest { public Guid TenantId => EmpresaId; }

public class UpdateRequisicaoHandler : IRequestHandler<UpdateRequisicaoCommand, RequisicaoDto>
{
    private readonly IEstoqueRepository _repo;
    private readonly IUnitOfWork _uow;

    public UpdateRequisicaoHandler(IEstoqueRepository repo, IUnitOfWork uow)
    {
        _repo = repo;
        _uow = uow;
    }

    public async Task<RequisicaoDto> Handle(UpdateRequisicaoCommand request, CancellationToken cancellationToken)
    {
        var requisicao = await _repo.GetRequisicaoByIdAsync(request.Id, request.EmpresaId, cancellationToken)
            ?? throw new KeyNotFoundException($"Requisição {request.Id} não encontrada.");

        requisicao.Motivo = request.Descricao;

        await _uow.SaveChangesAsync(cancellationToken);

        return new RequisicaoDto(
            requisicao.Id, requisicao.Numero, requisicao.ObraId, requisicao.AlmoxarifadoId,
            requisicao.Motivo, requisicao.Status, requisicao.Status.ToString(),
            requisicao.SolicitanteId, requisicao.CreatedAt);
    }
}
