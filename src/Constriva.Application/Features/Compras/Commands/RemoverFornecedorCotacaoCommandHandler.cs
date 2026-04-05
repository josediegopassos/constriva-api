using MediatR;
using Constriva.Application.Common.Behaviors;
using Constriva.Domain.Interfaces.Repositories;

namespace Constriva.Application.Features.Compras.Commands;

public record RemoverFornecedorCotacaoCommand(
    Guid CotacaoId,
    Guid FornecedorId,
    Guid EmpresaId) : IRequest, ITenantRequest;

public class RemoverFornecedorCotacaoHandler : IRequestHandler<RemoverFornecedorCotacaoCommand>
{
    private readonly IComprasRepository _repo;
    private readonly IUnitOfWork _uow;

    public RemoverFornecedorCotacaoHandler(IComprasRepository repo, IUnitOfWork uow)
    {
        _repo = repo;
        _uow = uow;
    }

    public async Task Handle(RemoverFornecedorCotacaoCommand request, CancellationToken ct)
    {
        var fc = await _repo.GetFornecedorCotacaoAsync(request.CotacaoId, request.FornecedorId, request.EmpresaId, ct)
            ?? throw new KeyNotFoundException(
                $"Fornecedor {request.FornecedorId} não encontrado na cotação {request.CotacaoId}.");

        fc.SoftDelete(Guid.Empty);
        await _uow.SaveChangesAsync(ct);
    }
}
