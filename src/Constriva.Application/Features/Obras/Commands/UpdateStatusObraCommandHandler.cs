using MediatR;
using Constriva.Application.Common.Behaviors;
using Constriva.Domain.Entities.Obras;
using Constriva.Domain.Enums;
using Constriva.Domain.Interfaces.Repositories;
using Constriva.Application.Features.Obras.DTOs;

namespace Constriva.Application.Features.Obras.Commands;

public record UpdateStatusObraCommand(Guid Id, Guid EmpresaId, Guid UsuarioId, StatusObraEnum Status, string? Observacao)
    : IRequest<Unit>, ITenantRequest;

public class UpdateStatusObraCommandHandler : IRequestHandler<UpdateStatusObraCommand, Unit>
{
    private static readonly Dictionary<StatusObraEnum, HashSet<StatusObraEnum>> TransicoesValidas = new()
    {
        [StatusObraEnum.Orcamento]   = [StatusObraEnum.Aprovada, StatusObraEnum.Cancelada],
        [StatusObraEnum.Aprovada]    = [StatusObraEnum.EmAndamento, StatusObraEnum.Cancelada],
        [StatusObraEnum.EmAndamento] = [StatusObraEnum.Paralisada, StatusObraEnum.Concluida, StatusObraEnum.Cancelada],
        [StatusObraEnum.Paralisada]  = [StatusObraEnum.EmAndamento, StatusObraEnum.Cancelada],
        [StatusObraEnum.Concluida]   = [],
        [StatusObraEnum.Cancelada]   = [],
    };

    private readonly IObraRepository _repo;
    private readonly IUnitOfWork _uow;
    public UpdateStatusObraCommandHandler(IObraRepository repo, IUnitOfWork uow) { _repo = repo; _uow = uow; }

    public async Task<Unit> Handle(UpdateStatusObraCommand r, CancellationToken ct)
    {
        var obra = await _repo.GetByIdAndEmpresaAsync(r.Id, r.EmpresaId, ct)
            ?? throw new KeyNotFoundException($"Obra {r.Id} não encontrada.");

        if (!TransicoesValidas.TryGetValue(obra.Status, out var permitidos) || !permitidos.Contains(r.Status))
            throw new InvalidOperationException(
                $"Transição de '{obra.Status}' para '{r.Status}' não é permitida.");

        var statusAnterior = obra.Status;
        obra.Status = r.Status;

        if (r.Status == StatusObraEnum.EmAndamento && obra.DataInicioReal == null)
            obra.DataInicioReal = DateTime.Today;
        if (r.Status == StatusObraEnum.Concluida && obra.DataFimReal == null)
            obra.DataFimReal = DateTime.Today;

        if (!string.IsNullOrEmpty(r.Observacao))
            obra.Observacoes = r.Observacao;
        obra.UpdatedAt = DateTime.UtcNow;

        _repo.Update(obra);

        await _repo.AddHistoricoAsync(new ObraHistorico
        {
            ObraId = obra.Id,
            EmpresaId = obra.EmpresaId,
            UsuarioId = r.UsuarioId,
            Acao = "StatusAlterado",
            Descricao = $"Status alterado de '{statusAnterior}' para '{r.Status}'.",
            ValorAnterior = statusAnterior.ToString(),
            ValorNovo = r.Status.ToString()
        }, ct);

        await _uow.SaveChangesAsync(ct);
        return Unit.Value;
    }
}
