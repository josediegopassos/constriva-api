using MediatR;
using Constriva.Application.Common.Behaviors;
using Constriva.Application.Common.Interfaces;
using Constriva.Domain.Entities.Estoque;
using Constriva.Domain.Enums;
using Constriva.Domain.Interfaces.Repositories;
using Constriva.Application.Features.Estoque.DTOs;

namespace Constriva.Application.Features.Estoque.Commands;

public record CreateRequisicaoCommand(Guid EmpresaId, Guid UsuarioId, CreateRequisicaoDto Dto)
    : IRequest<RequisicaoDto>, ITenantRequest { public Guid TenantId => EmpresaId; }

public class CreateRequisicaoHandler : IRequestHandler<CreateRequisicaoCommand, RequisicaoDto>
{
    private readonly IEstoqueRepository _repo;
    private readonly IObraRepository _obraRepo;
    private readonly IMaterialRepository _materialRepo;
    private readonly IUnitOfWork _uow;

    public CreateRequisicaoHandler(IEstoqueRepository repo, IObraRepository obraRepo, IMaterialRepository materialRepo, IUnitOfWork uow)
    {
        _repo = repo;
        _obraRepo = obraRepo;
        _materialRepo = materialRepo;
        _uow = uow;
    }

    public async Task<RequisicaoDto> Handle(CreateRequisicaoCommand request, CancellationToken cancellationToken)
    {
        var dto = request.Dto;

        _ = await _obraRepo.GetByIdAndEmpresaAsync(dto.ObraId, request.EmpresaId, cancellationToken)
            ?? throw new KeyNotFoundException($"Obra {dto.ObraId} não encontrada.");

        _ = await _repo.GetAlmoxarifadoByIdAsync(dto.AlmoxarifadoId, request.EmpresaId, cancellationToken)
            ?? throw new KeyNotFoundException($"Almoxarifado {dto.AlmoxarifadoId} não encontrado.");

        var requisicao = new RequisicaoMaterial
        {
            EmpresaId = request.EmpresaId,
            ObraId = dto.ObraId,
            FaseObraId = dto.FaseObraId,
            AlmoxarifadoId = dto.AlmoxarifadoId,
            Numero = $"REQ-{DateTime.UtcNow:yyyyMMddHHmmss}",
            Motivo = dto.Motivo,
            DataNecessidade = dto.DataNecessidade,
            Observacoes = dto.Observacoes,
            Status = StatusRequisicaoEnum.Pendente,
            SolicitanteId = request.UsuarioId
        };

        await _repo.AddRequisicaoAsync(requisicao, cancellationToken);
        await _uow.SaveChangesAsync(cancellationToken);

        if (dto.Itens != null)
        {
            foreach (var itemDto in dto.Itens)
            {
                _ = await _materialRepo.GetByIdAndEmpresaAsync(itemDto.MaterialId, request.EmpresaId, cancellationToken)
                    ?? throw new KeyNotFoundException($"Material {itemDto.MaterialId} não encontrado.");

                if (itemDto.QuantidadeSolicitada <= 0)
                    throw new InvalidOperationException("Quantidade deve ser maior que zero.");

                await _repo.AddItemRequisicaoAsync(new ItemRequisicao
                {
                    EmpresaId = request.EmpresaId,
                    RequisicaoId = requisicao.Id,
                    MaterialId = itemDto.MaterialId,
                    QuantidadeSolicitada = itemDto.QuantidadeSolicitada,
                    PrecoReferencia = itemDto.PrecoReferencia,
                    Observacao = itemDto.Observacao
                }, cancellationToken);
            }

            await _uow.SaveChangesAsync(cancellationToken);
        }

        return ToDto(requisicao);
    }

    internal static RequisicaoDto ToDto(RequisicaoMaterial r, string solicitanteNome = "") => new(
        r.Id, r.Numero, r.ObraId, r.FaseObraId, r.AlmoxarifadoId,
        r.Motivo, r.Status, r.Status.ToString(),
        r.SolicitanteId, solicitanteNome, r.AprovadorId,
        r.DataNecessidade, r.DataAprovacao,
        r.MotivoRejeicao, r.Observacoes, r.CreatedAt);
}
