using MediatR;
using Constriva.Application.Common.Behaviors;
using Constriva.Application.Common.Interfaces;
using Constriva.Domain.Entities.Cronograma;
using Constriva.Domain.Enums;
using Constriva.Domain.Interfaces.Repositories;
using Constriva.Application.Features.Cronograma.DTOs;

namespace Constriva.Application.Features.Cronograma.Commands;

public record UpdatePredecessorasCommand(Guid AtividadeId, Guid EmpresaId, IEnumerable<Guid> Predecessoras)
    : IRequest<Unit>, ITenantRequest { public Guid TenantId => EmpresaId; }

public class UpdatePredecessorasCommandHandler : IRequestHandler<UpdatePredecessorasCommand, Unit>
{
    private readonly ICronogramaRepository _repo;
    private readonly IUnitOfWork _uow;

    public UpdatePredecessorasCommandHandler(ICronogramaRepository repo, IUnitOfWork uow)
    {
        _repo = repo;
        _uow = uow;
    }

    public async Task<Unit> Handle(UpdatePredecessorasCommand request, CancellationToken cancellationToken)
    {
        var atividade = await _repo.GetAtividadeByIdAsync(request.AtividadeId, request.EmpresaId, cancellationToken)
            ?? throw new KeyNotFoundException($"Atividade {request.AtividadeId} não encontrada.");

        var novasPredecessoras = request.Predecessoras.Distinct().ToList();

        // 1. Valida existência de cada predecessora e pertença ao mesmo cronograma
        foreach (var predecessoraId in novasPredecessoras)
        {
            if (predecessoraId == request.AtividadeId)
                throw new InvalidOperationException("Uma atividade não pode ser predecessora de si mesma.");

            var predecessora = await _repo.GetAtividadeByIdAsync(predecessoraId, request.EmpresaId, cancellationToken)
                ?? throw new KeyNotFoundException($"Atividade predecessora {predecessoraId} não encontrada.");

            if (predecessora.CronogramaId != atividade.CronogramaId)
                throw new InvalidOperationException(
                    $"Atividade predecessora {predecessoraId} pertence a um cronograma diferente.");
        }

        // 2. Detecção de ciclos via DFS
        // Carrega todos os vínculos existentes do cronograma para construir o grafo
        var vinculos = await _repo.GetVinculosAsync(atividade.CronogramaId, cancellationToken);

        // Grafo: origem (predecessor) → lista de destinos (dependentes)
        var grafo = vinculos
            .GroupBy(v => v.AtividadeOrigemId)
            .ToDictionary(g => g.Key, g => g.Select(v => v.AtividadeDestinoId).ToList());

        // Para cada nova predecessora Pi: verificar se a atividade X já é acessível a partir de Pi
        // (i.e., há caminho Pi → ... → X no grafo atual).
        // Se sim, adicionar Pi → X criaria Pi → X → ... → Pi = ciclo.
        foreach (var predecessoraId in novasPredecessoras)
        {
            if (TemCaminhoParaAlvo(predecessoraId, request.AtividadeId, grafo))
                throw new InvalidOperationException(
                    $"A atividade predecessora {predecessoraId} introduziria um ciclo no cronograma. " +
                    $"Verifique as dependências existentes.");
        }

        // 3. Substitui os vínculos atuais pelos novos
        // Remove vínculos existentes da atividade (onde ela é destino)
        foreach (var vinculo in atividade.Predecessoras.ToList())
            atividade.Predecessoras.Remove(vinculo);

        // Adiciona novos vínculos
        foreach (var predecessoraId in novasPredecessoras)
        {
            atividade.Predecessoras.Add(new VinculoAtividade
            {
                EmpresaId = request.EmpresaId,
                AtividadeOrigemId = predecessoraId,
                AtividadeDestinoId = atividade.Id,
                Tipo = TipoVinculoEnum.FS
            });
        }

        await _uow.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }

    /// <summary>
    /// DFS: verifica se há caminho de <paramref name="inicio"/> até <paramref name="alvo"/>
    /// no grafo de dependências (origem → destino).
    /// Retorna true se a atividade alvo é acessível a partir de inicio, indicando ciclo potencial.
    /// </summary>
    private static bool TemCaminhoParaAlvo(Guid inicio, Guid alvo, Dictionary<Guid, List<Guid>> grafo)
    {
        var visitados = new HashSet<Guid>();
        var stack = new Stack<Guid>();
        stack.Push(inicio);

        while (stack.Count > 0)
        {
            var atual = stack.Pop();
            if (atual == alvo) return true;
            if (!visitados.Add(atual)) continue;

            if (grafo.TryGetValue(atual, out var destinos))
                foreach (var d in destinos)
                    stack.Push(d);
        }

        return false;
    }
}
