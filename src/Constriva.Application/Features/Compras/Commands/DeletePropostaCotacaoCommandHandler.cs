using MediatR;
using Constriva.Application.Common.Behaviors;
using Constriva.Domain.Interfaces.Repositories;

namespace Constriva.Application.Features.Compras.Commands;

public record DeletePropostaCotacaoCommand(Guid PropostaId, Guid EmpresaId) : IRequest, ITenantRequest;

public class DeletePropostaCotacaoHandler : IRequestHandler<DeletePropostaCotacaoCommand>
{
    private readonly IComprasRepository _repo;
    private readonly IUnitOfWork _uow;

    public DeletePropostaCotacaoHandler(IComprasRepository repo, IUnitOfWork uow)
    {
        _repo = repo;
        _uow = uow;
    }

    public async Task Handle(DeletePropostaCotacaoCommand request, CancellationToken ct)
    {
        var proposta = await _repo.GetPropostaByIdAsync(request.PropostaId, request.EmpresaId, ct)
            ?? throw new KeyNotFoundException($"Proposta {request.PropostaId} não encontrada.");

        proposta.SoftDelete(Guid.Empty);
        await _uow.SaveChangesAsync(ct);
    }
}
