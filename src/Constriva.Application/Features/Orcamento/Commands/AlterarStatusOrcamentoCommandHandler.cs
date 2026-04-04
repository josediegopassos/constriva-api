using MediatR;
using Constriva.Application.Common.Behaviors;
using Constriva.Domain.Enums;
using Constriva.Domain.Interfaces.Repositories;

namespace Constriva.Application.Features.Orcamento.Commands;

public record AlterarStatusOrcamentoDto(StatusOrcamentoEnum Status, string? Observacao = null);

public record AlterarStatusOrcamentoCommand(Guid Id, Guid EmpresaId, Guid UsuarioId, StatusOrcamentoEnum Status, string? Observacao)
    : IRequest<Unit>, ITenantRequest;

public class AlterarStatusOrcamentoHandler : IRequestHandler<AlterarStatusOrcamentoCommand, Unit>
{
    private readonly IOrcamentoRepository _repo;
    private readonly IUnitOfWork _uow;

    public AlterarStatusOrcamentoHandler(IOrcamentoRepository repo, IUnitOfWork uow)
    { _repo = repo; _uow = uow; }

    public async Task<Unit> Handle(AlterarStatusOrcamentoCommand r, CancellationToken ct)
    {
        var orcamento = await _repo.GetByIdAsync(r.Id, r.EmpresaId, ct)
            ?? throw new KeyNotFoundException($"Orçamento {r.Id} não encontrado.");

        if (orcamento.Status == r.Status)
            throw new InvalidOperationException($"Orçamento já está com status {r.Status}.");

        orcamento.Status = r.Status;

        if (r.Status == StatusOrcamentoEnum.Aprovado)
        {
            orcamento.AprovadoPor = r.UsuarioId;
            orcamento.DataAprovacao = DateTime.UtcNow;
        }

        if (r.Observacao != null)
            orcamento.Observacoes = r.Observacao;

        orcamento.UpdatedAt = DateTime.UtcNow;
        _repo.Update(orcamento);
        await _uow.SaveChangesAsync(ct);
        return Unit.Value;
    }
}
