using FluentValidation;
using MediatR;
using Constriva.Application.Common.Behaviors;
using Constriva.Application.Common.Interfaces;
using Constriva.Domain.Entities.Orcamento;
using Constriva.Domain.Enums;
using Constriva.Domain.Interfaces.Repositories;
using Constriva.Application.Features.Orcamento.DTOs;

namespace Constriva.Application.Features.Orcamento.Commands;

public record CreateGrupoOrcamentoCommand(Guid OrcamentoId, Guid EmpresaId, CreateGrupoDto Dto)
    : IRequest<GrupoOrcamentoDto>, ITenantRequest;

public class CreateGrupoOrcamentoHandler : IRequestHandler<CreateGrupoOrcamentoCommand, GrupoOrcamentoDto>
{
    private readonly IGrupoOrcamentoRepository _grupoRepo;
    private readonly IOrcamentoRepository _orcRepo;
    private readonly IUnitOfWork _uow;

    public CreateGrupoOrcamentoHandler(
        IGrupoOrcamentoRepository grupoRepo, IOrcamentoRepository orcRepo, IUnitOfWork uow)
    {
        _grupoRepo = grupoRepo;
        _orcRepo = orcRepo;
        _uow = uow;
    }

    public async Task<GrupoOrcamentoDto> Handle(CreateGrupoOrcamentoCommand request, CancellationToken ct)
    {
        var orcamento = await _orcRepo.GetByIdAsync(request.OrcamentoId, request.EmpresaId, ct)
            ?? throw new KeyNotFoundException("Orçamento não encontrado.");

        if (orcamento.Status == StatusOrcamentoEnum.Aprovado)
            throw new InvalidOperationException("Não é possível adicionar grupos a um orçamento aprovado.");

        var maxOrdem = await _grupoRepo.GetMaxOrdemAsync(request.OrcamentoId, ct);
        var ordem = maxOrdem + 1;

        var grupo = new GrupoOrcamento
        {
            EmpresaId = request.EmpresaId,
            OrcamentoId = request.OrcamentoId,
            GrupoPaiId = request.Dto.GrupoPaiId,
            Nome = request.Dto.Nome,
            Codigo = ordem.ToString("D2"),
            Ordem = ordem
        };

        await _grupoRepo.AddAsync(grupo, ct);
        await _uow.SaveChangesAsync(ct);

        return new GrupoOrcamentoDto(
            grupo.Id, grupo.Codigo, grupo.Nome, grupo.Ordem,
            0, 0, grupo.GrupoPaiId,
            Enumerable.Empty<GrupoOrcamentoDto>(),
            Enumerable.Empty<ItemOrcamentoDto>());
    }
}
