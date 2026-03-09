using MediatR;
using Constriva.Application.Common.Behaviors;
using Constriva.Application.Common.Interfaces;
using Constriva.Domain.Entities.Orcamento;
using Constriva.Domain.Enums;
using Constriva.Domain.Interfaces.Repositories;
using Constriva.Application.Features.Orcamento.DTOs;

namespace Constriva.Application.Features.Orcamento.Commands;

public record DuplicarOrcamentoCommand(
    Guid Id,
    Guid EmpresaId,
    Guid UsuarioId,
    string NovoNome) : IRequest<OrcamentoResumoDto>, ITenantRequest;

public class DuplicarOrcamentoHandler : IRequestHandler<DuplicarOrcamentoCommand, OrcamentoResumoDto>
{
    private readonly IOrcamentoRepository _repo;
    private readonly IGrupoOrcamentoRepository _grupoRepo;
    private readonly IItemOrcamentoRepository _itemRepo;
    private readonly IUnitOfWork _uow;

    public DuplicarOrcamentoHandler(
        IOrcamentoRepository repo,
        IGrupoOrcamentoRepository grupoRepo,
        IItemOrcamentoRepository itemRepo,
        IUnitOfWork uow)
    {
        _repo = repo;
        _grupoRepo = grupoRepo;
        _itemRepo = itemRepo;
        _uow = uow;
    }

    public async Task<OrcamentoResumoDto> Handle(DuplicarOrcamentoCommand request, CancellationToken ct)
    {
        var original = await _repo.GetByIdAsync(request.Id, request.EmpresaId, ct)
            ?? throw new KeyNotFoundException("Orçamento não encontrado.");

        var maxVersao = await _repo.GetMaxVersaoAsync(original.ObraId, request.EmpresaId, ct);
        var versao = maxVersao + 1;
        var obraIdShort = original.ObraId.ToString()[..8].ToUpper();
        var codigo = $"ORC-{obraIdShort}-V{versao}";

        var novo = new Domain.Entities.Orcamento.Orcamento
        {
            ObraId = original.ObraId,
            EmpresaId = request.EmpresaId,
            Codigo = codigo,
            Nome = request.NovoNome,
            Versao = versao,
            Status = StatusOrcamentoEnum.Rascunho,
            BDI = original.BDI,
            DataReferencia = original.DataReferencia,
            Descricao = original.Descricao,
            BaseOrcamentaria = original.BaseOrcamentaria,
            Localidade = original.Localidade,
            ValorCustoDirecto = original.ValorCustoDirecto,
            ValorBDI = original.ValorBDI,
            ValorTotal = original.ValorTotal
        };

        await _repo.AddAsync(novo, ct);

        // Copy groups and items (recursive, BFS to handle sub-groups)
        var allGrupos = (await _grupoRepo.GetByOrcamentoAsync(original.Id, ct)).ToList();
        var oldToNew = new Dictionary<Guid, Guid>(); // old GrupoId → new GrupoId

        // Process groups in BFS order so parents are created before children
        var queue = new Queue<GrupoOrcamento>(allGrupos.Where(g => g.GrupoPaiId == null));
        while (queue.Count > 0)
        {
            var g = queue.Dequeue();
            var novoGrupo = new GrupoOrcamento
            {
                OrcamentoId = novo.Id,
                EmpresaId = request.EmpresaId,
                GrupoPaiId = g.GrupoPaiId.HasValue ? oldToNew[g.GrupoPaiId.Value] : null,
                Codigo = g.Codigo,
                Nome = g.Nome,
                Ordem = g.Ordem,
                ValorTotal = g.ValorTotal,
                PercentualTotal = g.PercentualTotal
            };
            await _grupoRepo.AddAsync(novoGrupo, ct);
            oldToNew[g.Id] = novoGrupo.Id;

            foreach (var filho in allGrupos.Where(x => x.GrupoPaiId == g.Id))
                queue.Enqueue(filho);
        }

        // Copy items for every group
        foreach (var g in allGrupos)
        {
            var itens = await _itemRepo.GetByGrupoAsync(g.Id, ct);
            foreach (var item in itens)
            {
                await _itemRepo.AddAsync(new ItemOrcamento
                {
                    OrcamentoId = novo.Id,
                    EmpresaId = request.EmpresaId,
                    GrupoId = oldToNew[g.Id],
                    Codigo = item.Codigo,
                    CodigoFonte = item.CodigoFonte,
                    Descricao = item.Descricao,
                    UnidadeMedida = item.UnidadeMedida,
                    Quantidade = item.Quantidade,
                    CustoUnitario = item.CustoUnitario,
                    BDI = item.BDI,
                    CustoComBDI = item.CustoComBDI,
                    Ordem = item.Ordem,
                    Fonte = item.Fonte
                }, ct);
            }
        }

        await _repo.AddHistoricoAsync(new OrcamentoHistorico
        {
            EmpresaId = request.EmpresaId,
            OrcamentoId = novo.Id,
            Descricao = $"Orçamento duplicado a partir de {original.Codigo}.",
            UsuarioId = request.UsuarioId
        }, ct);
        await _uow.SaveChangesAsync(ct);

        return OrcamentoMapper.ToResumoDto(novo, allGrupos.Count);
    }
}
