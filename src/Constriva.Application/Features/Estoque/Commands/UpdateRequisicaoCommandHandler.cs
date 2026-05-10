using MediatR;

using Constriva.Application.Common.Interfaces;
using Constriva.Domain.Interfaces.Repositories;
using Constriva.Application.Features.Estoque.DTOs;

namespace Constriva.Application.Features.Estoque.Commands;

public record UpdateRequisicaoCommand(Guid Id, Guid EmpresaId, UpdateRequisicaoDto Dto)
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
        var dto = request.Dto;
        var requisicao = await _repo.GetRequisicaoByIdAsync(request.Id, request.EmpresaId, cancellationToken)
            ?? throw new KeyNotFoundException($"Requisição {request.Id} não encontrada.");

        requisicao.Motivo = dto.Motivo;
        requisicao.DataNecessidade = dto.DataNecessidade;
        requisicao.Observacoes = dto.Observacoes;

        await _uow.SaveChangesAsync(cancellationToken);

        return CreateRequisicaoHandler.ToDto(requisicao);
    }
}
