using MediatR;
using Constriva.Application.Common.Behaviors;
using Constriva.Application.Common.Interfaces;
using Constriva.Domain.Enums;
using Constriva.Domain.Interfaces.Repositories;
using Constriva.Application.Features.Cronograma.DTOs;

namespace Constriva.Application.Features.Cronograma.Commands;

public record UpdateAtividadeCommand(
    Guid Id,
    Guid EmpresaId,
    UpdateAtividadeDto Dto)
    : IRequest<AtividadeDto>, ITenantRequest { public Guid TenantId => EmpresaId; }

public class UpdateAtividadeCommandHandler : IRequestHandler<UpdateAtividadeCommand, AtividadeDto>
{
    private readonly ICronogramaRepository _repo;
    private readonly IUnitOfWork _uow;

    public UpdateAtividadeCommandHandler(ICronogramaRepository repo, IUnitOfWork uow)
    {
        _repo = repo;
        _uow = uow;
    }

    public async Task<AtividadeDto> Handle(UpdateAtividadeCommand request, CancellationToken cancellationToken)
    {
        var atividade = await _repo.GetAtividadeByIdAsync(request.Id, request.EmpresaId, cancellationToken)
            ?? throw new KeyNotFoundException("Atividade não encontrada.");

        var dto = request.Dto;

        atividade.Nome                  = dto.Nome;
        atividade.Descricao             = dto.Descricao;
        atividade.Ordem                 = dto.Ordem;
        atividade.Nivel                 = dto.Nivel;
        atividade.AtividadePaiId        = dto.AtividadePaiId;
        atividade.FaseObraId            = dto.FaseObraId;
        atividade.EAgrupadoa            = dto.EAgrupadora;
        atividade.EMarcador             = dto.EMarcador;
        atividade.DataInicioPlanejada   = dto.DataInicio;
        atividade.DataFimPlanejada      = dto.DataFim;
        atividade.DataInicioReal        = dto.DataInicioReal;
        atividade.DataFimReal           = dto.DataFimReal;
        atividade.DataInicioReprogramada = dto.DataInicioReprogramada;
        atividade.DataFimReprogramada   = dto.DataFimReprogramada;
        atividade.DuracaoDias           = dto.DuracaoDias > 0
            ? dto.DuracaoDias
            : (int)(dto.DataFim - dto.DataInicio).TotalDays;
        atividade.PercentualConcluido   = dto.PercentualConcluido;
        atividade.CustoOrcado           = dto.CustoOrcado;
        atividade.CustoRealizado        = dto.CustoRealizado;
        atividade.ResponsavelId         = dto.ResponsavelId;
        atividade.Cor                   = dto.Cor;
        atividade.Observacoes           = dto.Observacoes;

        atividade.Status = dto.PercentualConcluido == 100
            ? StatusAtividadeEnum.Concluida
            : dto.PercentualConcluido > 0
                ? StatusAtividadeEnum.EmAndamento
                : StatusAtividadeEnum.NaoIniciada;

        await _uow.SaveChangesAsync(cancellationToken);

        return new AtividadeDto(
            atividade.Id, atividade.Nome, atividade.Descricao, atividade.Ordem,
            atividade.DataInicioPlanejada, atividade.DataFimPlanejada,
            atividade.DataInicioReal, atividade.DataFimReal,
            atividade.PercentualConcluido, atividade.Status, atividade.NoCaminhosCritico,
            atividade.Predecessoras?.Select(p => p.AtividadeOrigemId) ?? []);
    }
}
