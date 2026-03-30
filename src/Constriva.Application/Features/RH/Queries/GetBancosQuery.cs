using MediatR;
using Constriva.Domain.Interfaces.Repositories;

namespace Constriva.Application.Features.RH;

public record BancoDto(int Id, string Nome);

public record GetBancosQuery() : IRequest<IEnumerable<BancoDto>>;

public class GetBancosHandler : IRequestHandler<GetBancosQuery, IEnumerable<BancoDto>>
{
    private readonly IRHRepository _repo;
    public GetBancosHandler(IRHRepository repo) => _repo = repo;

    public async Task<IEnumerable<BancoDto>> Handle(GetBancosQuery r, CancellationToken ct)
    {
        var items = await _repo.GetBancosAsync(ct);
        return items.Select(b => new BancoDto(b.Id, $"{b.Codigo} - {b.Nome}"));
    }
}
