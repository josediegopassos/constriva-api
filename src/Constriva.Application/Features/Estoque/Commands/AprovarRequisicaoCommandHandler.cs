using MediatR;
using Constriva.Application.Common.Behaviors;
using Constriva.Application.Common.Interfaces;
using Constriva.Domain.Enums;
using Constriva.Domain.Interfaces.Repositories;
using Constriva.Application.Features.Estoque.DTOs;

namespace Constriva.Application.Features.Estoque.Commands;

public record AprovarRequisicaoCommand(Guid Id, Guid EmpresaId, Guid UsuarioId)
    : IRequest<RequisicaoDto>, ITenantRequest { public Guid TenantId => EmpresaId; }

public class AprovarRequisicaoHandler : IRequestHandler<AprovarRequisicaoCommand, RequisicaoDto>
{
    private readonly IEstoqueRepository _repo;
    private readonly IUnitOfWork _uow;

    public AprovarRequisicaoHandler(IEstoqueRepository repo, IUnitOfWork uow)
    {
        _repo = repo;
        _uow = uow;
    }

    public async Task<RequisicaoDto> Handle(AprovarRequisicaoCommand request, CancellationToken cancellationToken)
    {
        var requisicao = await _repo.GetRequisicaoByIdAsync(request.Id, request.EmpresaId, cancellationToken)
            ?? throw new KeyNotFoundException($"Requisição {request.Id} não encontrada.");

        if (requisicao.Status is StatusRequisicaoEnum.Aprovada)
            throw new InvalidOperationException("Requisição já está aprovada.");

        if (requisicao.Status is StatusRequisicaoEnum.Cancelada or StatusRequisicaoEnum.Atendida)
            throw new InvalidOperationException($"Não é possível aprovar uma requisição com status '{requisicao.Status}'.");

        requisicao.Status = StatusRequisicaoEnum.Aprovada;
        requisicao.AprovadorId = request.UsuarioId;
        requisicao.DataAprovacao = DateTime.UtcNow;

        await _uow.SaveChangesAsync(cancellationToken);

        return CreateRequisicaoHandler.ToDto(requisicao);
    }
}
