using MediatR;

namespace Constriva.Application.Common.Implementations;

public class PaginatedQuery<T> : IRequest<PaginatedResult<T>>
{
    public int Page { get; init; } = 1;
    public int PageSize { get; init; } = 20;
    public string? Search { get; init; }
    public string? OrderBy { get; init; }
    public bool OrderDesc { get; init; } = false;
}
