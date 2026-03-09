using MediatR;
using Constriva.Application.Common.Behaviors;
using Constriva.Application.Common.Interfaces;
using Constriva.Domain.Entities.Cronograma;
using Constriva.Domain.Enums;
using Constriva.Domain.Interfaces.Repositories;
using Constriva.Application.Features.Cronograma.DTOs;

namespace Constriva.Application.Features.Cronograma.Commands;

public record CreateAtividadeCommand(Guid ObraId, Guid EmpresaId, CreateAtividadeDto Dto)
    : IRequest<AtividadeDto>, ITenantRequest;

public class CreateAtividadeCommandHandler : IRequestHandler<CreateAtividadeCommand, AtividadeDto>
{
    private readonly ICronogramaRepository _repo;
    private readonly IUnitOfWork _uow;

    public CreateAtividadeCommandHandler(ICronogramaRepository repo, IUnitOfWork uow)
    {
        _repo = repo;
        _uow = uow;
    }

    public async Task<AtividadeDto> Handle(CreateAtividadeCommand request, CancellationToken cancellationToken)
    {
        var cronograma = await _repo.GetByObraAsync(request.ObraId, request.EmpresaId, cancellationToken)
            ?? throw new KeyNotFoundException("Cronograma não encontrado para esta obra.");

        var dto = request.Dto;

        // Respeita Dto.Ordem quando explicitamente informado (> 0); caso contrário, auto-incrementa.
        int ordem;
        if (dto.Ordem > 0)
        {
            ordem = dto.Ordem;
        }
        else
        {
            var maxOrdem = await _repo.GetMaxOrdemAsync(cronograma.Id, cancellationToken);
            ordem = maxOrdem + 1;
        }

        var atividade = new AtividadeCronograma
        {
            CronogramaId = cronograma.Id,
            EmpresaId = request.EmpresaId,
            Codigo = ordem.ToString("D3"),
            Nome = dto.Nome,
            Descricao = dto.Descricao,
            Ordem = ordem,
            DataInicioPlanejada = dto.DataInicioPrevista,
            DataFimPlanejada = dto.DataFimPrevista,
            DuracaoDias = (int)dto.DuracaoDias,
            PercentualConcluido = 0,
            Status = StatusAtividadeEnum.NaoIniciada
        };

        await _repo.AddAtividadeAsync(atividade, cancellationToken);
        // Após AddAtividadeAsync o Id está disponível (atribuído pelo EF ou pelo construtor da entidade)

        var predecessorasIds = new List<Guid>();
        if (dto.Predecessoras != null && dto.Predecessoras.Any())
        {
            foreach (var predecessoraId in dto.Predecessoras.Distinct())
            {
                // Valida existência e pertença à empresa
                var predecessora = await _repo.GetAtividadeByIdAsync(predecessoraId, request.EmpresaId, cancellationToken)
                    ?? throw new KeyNotFoundException($"Atividade predecessora {predecessoraId} não encontrada.");

                // Valida pertença ao mesmo cronograma
                if (predecessora.CronogramaId != cronograma.Id)
                    throw new InvalidOperationException(
                        $"A atividade predecessora {predecessoraId} pertence a um cronograma diferente.");

                // Nota: para uma NOVA atividade sem sucessoras, não é possível introduzir ciclo.
                // Detecção completa de ciclos é aplicável em UpdatePredecessoras (atividade existente).
                atividade.Predecessoras.Add(new VinculoAtividade
                {
                    EmpresaId = request.EmpresaId,
                    AtividadeOrigemId = predecessoraId,
                    AtividadeDestinoId = atividade.Id,
                    Tipo = TipoVinculoEnum.FS
                });

                predecessorasIds.Add(predecessoraId);
            }
        }

        await _uow.SaveChangesAsync(cancellationToken);

        return new AtividadeDto(
            atividade.Id, atividade.Nome, atividade.Descricao, atividade.Ordem,
            atividade.DataInicioPlanejada, atividade.DataFimPlanejada,
            null, null, 0, StatusAtividadeEnum.NaoIniciada, false,
            predecessorasIds);
    }
}
