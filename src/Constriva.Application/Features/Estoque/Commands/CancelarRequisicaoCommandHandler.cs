using MediatR;
using Constriva.Application.Common.Behaviors;
using Constriva.Application.Common.Interfaces;
using Constriva.Domain.Enums;
using Constriva.Domain.Interfaces.Repositories;
using Constriva.Application.Features.Estoque.DTOs;

namespace Constriva.Application.Features.Estoque.Commands;

public record CancelarRequisicaoCommand(Guid Id, Guid EmpresaId, Guid UsuarioId, string Motivo)
    : IRequest<RequisicaoDto>, ITenantRequest { public Guid TenantId => EmpresaId; }

public class CancelarRequisicaoHandler : IRequestHandler<CancelarRequisicaoCommand, RequisicaoDto>
{
    private readonly IEstoqueRepository _repo;
    private readonly IUnitOfWork _uow;

    public CancelarRequisicaoHandler(IEstoqueRepository repo, IUnitOfWork uow)
    {
        _repo = repo;
        _uow = uow;
    }

    public async Task<RequisicaoDto> Handle(CancelarRequisicaoCommand request, CancellationToken cancellationToken)
    {
        var requisicao = await _repo.GetRequisicaoByIdAsync(request.Id, request.EmpresaId, cancellationToken)
            ?? throw new KeyNotFoundException($"Requisição {request.Id} não encontrada.");

        if (requisicao.Status is StatusRequisicaoEnum.Atendida or StatusRequisicaoEnum.Cancelada)
            throw new InvalidOperationException($"Não é possível cancelar uma requisição com status '{requisicao.Status}'.");

        requisicao.Status = StatusRequisicaoEnum.Cancelada;
        requisicao.MotivoRejeicao = request.Motivo;

        await _uow.SaveChangesAsync(cancellationToken);

        return CreateRequisicaoHandler.ToDto(requisicao);
    }
}
