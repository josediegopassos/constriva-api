using MediatR;
using Constriva.Application.Common.Behaviors;
using Constriva.Domain.Enums;
using Constriva.Domain.Interfaces.Repositories;

namespace Constriva.Application.Features.Compras.Commands;

public record DeleteCotacaoCommand(Guid Id, Guid EmpresaId) : IRequest, ITenantRequest;

public class DeleteCotacaoHandler : IRequestHandler<DeleteCotacaoCommand>
{
    private readonly IComprasRepository _repo;
    private readonly IUnitOfWork _uow;

    public DeleteCotacaoHandler(IComprasRepository repo, IUnitOfWork uow)
    {
        _repo = repo;
        _uow = uow;
    }

    public async Task Handle(DeleteCotacaoCommand request, CancellationToken ct)
    {
        var cotacao = await _repo.GetCotacaoByIdAsync(request.Id, request.EmpresaId, ct)
            ?? throw new KeyNotFoundException($"Cotação {request.Id} não encontrada.");

        cotacao.SoftDelete(Guid.Empty);
        await _uow.SaveChangesAsync(ct);
    }
}
