using MediatR;
using Constriva.Application.Common.Behaviors;
using Constriva.Domain.Interfaces.Repositories;
using Constriva.Application.Features.Estoque.DTOs;

namespace Constriva.Application.Features.Estoque.Queries;

public record GetRequisicaoByIdQuery(Guid Id, Guid EmpresaId) : IRequest<RequisicaoDetalheDto?>, ITenantRequest;

public class GetRequisicaoByIdHandler : IRequestHandler<GetRequisicaoByIdQuery, RequisicaoDetalheDto?>
{
    private readonly IEstoqueRepository _repo;
    private readonly IUsuarioRepository _usuarioRepo;

    public GetRequisicaoByIdHandler(IEstoqueRepository repo, IUsuarioRepository usuarioRepo)
    {
        _repo = repo;
        _usuarioRepo = usuarioRepo;
    }

    public async Task<RequisicaoDetalheDto?> Handle(GetRequisicaoByIdQuery r, CancellationToken ct)
    {
        var req = await _repo.GetRequisicaoByIdAsync(r.Id, r.EmpresaId, ct);
        if (req == null) return null;

        var solicitante = req.CreatedBy.HasValue
            ? await _usuarioRepo.GetByIdAsync(req.CreatedBy.Value, ct)
            : null;

        return new RequisicaoDetalheDto(
            req.Id, req.Numero, req.ObraId, req.FaseObraId, req.AlmoxarifadoId,
            req.Motivo, req.Status, req.Status.ToString(),
            req.SolicitanteId, solicitante?.Nome ?? "",
            req.AprovadorId,
            req.DataRequisicao, req.DataNecessidade, req.DataAprovacao,
            req.MotivoRejeicao, req.Observacoes, req.CreatedAt,
            req.Itens.Select(i => new ItemRequisicaoDto(
                i.Id,
                i.MaterialId,
                i.Material?.Codigo ?? "",
                i.Material?.Nome ?? "",
                i.Material?.UnidadeMedida ?? "",
                i.QuantidadeSolicitada,
                i.QuantidadeAtendida,
                i.PrecoReferencia,
                i.Observacao)));
    }
}
