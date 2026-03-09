using MediatR;
using Constriva.Application.Common.Behaviors;
using Constriva.Application.Common.Interfaces;
using Constriva.Domain.Entities.Orcamento;
using Constriva.Domain.Enums;
using Constriva.Domain.Interfaces.Repositories;
using Constriva.Application.Features.Orcamento.DTOs;

namespace Constriva.Application.Features.Orcamento.Commands;

public record UpdateGrupoOrcamentoCommand(
    Guid GrupoId,
    Guid EmpresaId,
    UpdateGrupoDto Dto) : IRequest<GrupoOrcamentoDto>, ITenantRequest;

public class UpdateGrupoOrcamentoHandler : IRequestHandler<UpdateGrupoOrcamentoCommand, GrupoOrcamentoDto>
{
    private readonly IGrupoOrcamentoRepository _grupoRepo;
    private readonly IUnitOfWork _uow;

    public UpdateGrupoOrcamentoHandler(IGrupoOrcamentoRepository grupoRepo, IUnitOfWork uow)
    {
        _grupoRepo = grupoRepo;
        _uow = uow;
    }

    public async Task<GrupoOrcamentoDto> Handle(UpdateGrupoOrcamentoCommand request, CancellationToken ct)
    {
        var grupo = await _grupoRepo.GetByIdAsync(request.GrupoId, request.EmpresaId, ct)
            ?? throw new KeyNotFoundException("Grupo não encontrado.");

        grupo.Nome = request.Dto.Nome;
        grupo.Ordem = request.Dto.Ordem;
        _grupoRepo.Update(grupo);
        await _uow.SaveChangesAsync(ct);

        return new GrupoOrcamentoDto(
            grupo.Id, grupo.Codigo, grupo.Nome, grupo.Ordem,
            grupo.ValorTotal, grupo.PercentualTotal, grupo.GrupoPaiId,
            Enumerable.Empty<GrupoOrcamentoDto>(),
            Enumerable.Empty<ItemOrcamentoDto>());
    }
}
