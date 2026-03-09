using MediatR;
using Constriva.Application.Common.Behaviors;
using Constriva.Application.Common.Interfaces;
using Constriva.Domain.Entities.Estoque;
using Constriva.Domain.Enums;
using Constriva.Domain.Interfaces.Repositories;
using Constriva.Application.Features.Estoque.DTOs;

namespace Constriva.Application.Features.Estoque.Commands;

public record CreateRequisicaoCommand(Guid EmpresaId, Guid UsuarioId, CreateRequisicaoDto Dto)
    : IRequest<RequisicaoDto>, ITenantRequest;

public class CreateRequisicaoHandler : IRequestHandler<CreateRequisicaoCommand, RequisicaoDto>
{
    private readonly IEstoqueRepository _repo;
    private readonly IObraRepository _obraRepo;
    private readonly IUnitOfWork _uow;

    public CreateRequisicaoHandler(IEstoqueRepository repo, IObraRepository obraRepo, IUnitOfWork uow)
    {
        _repo = repo;
        _obraRepo = obraRepo;
        _uow = uow;
    }

    public async Task<RequisicaoDto> Handle(CreateRequisicaoCommand request, CancellationToken cancellationToken)
    {
        var dto = request.Dto;

        // Valida existência e pertença da obra ao tenant
        _ = await _obraRepo.GetByIdAndEmpresaAsync(dto.ObraId, request.EmpresaId, cancellationToken)
            ?? throw new KeyNotFoundException($"Obra {dto.ObraId} não encontrada.");

        // Valida existência e pertença do almoxarifado ao tenant
        _ = await _repo.GetAlmoxarifadoByIdAsync(dto.AlmoxarifadoId, request.EmpresaId, cancellationToken)
            ?? throw new KeyNotFoundException($"Almoxarifado {dto.AlmoxarifadoId} não encontrado.");

        var numero = $"REQ-{DateTime.UtcNow:yyyyMMddHHmmss}";

        var requisicao = new RequisicaoMaterial
        {
            EmpresaId = request.EmpresaId,
            ObraId = dto.ObraId,
            AlmoxarifadoId = dto.AlmoxarifadoId,
            Numero = numero,
            Motivo = dto.Motivo,
            Status = StatusRequisicaoEnum.Pendente,
            SolicitanteId = request.UsuarioId
        };

        await _repo.AddRequisicaoAsync(requisicao, cancellationToken);
        await _uow.SaveChangesAsync(cancellationToken);

        return new RequisicaoDto(
            requisicao.Id, requisicao.Numero, requisicao.ObraId, requisicao.AlmoxarifadoId,
            requisicao.Motivo, requisicao.Status, requisicao.Status.ToString(),
            requisicao.SolicitanteId, requisicao.CreatedAt);
    }
}
