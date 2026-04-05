using MediatR;
using Constriva.Application.Common.Behaviors;
using Constriva.Application.Features.Lens.Interfaces;

namespace Constriva.Application.Features.Lens.Queries;

public record GetLogProcessamentoQuery(
    Guid ProcessamentoId,
    Guid EmpresaId)
    : IRequest<object?>, ITenantRequest;

public class GetLogProcessamentoHandler : IRequestHandler<GetLogProcessamentoQuery, object?>
{
    private readonly ILensLogService _logService;

    public GetLogProcessamentoHandler(ILensLogService logService)
    {
        _logService = logService;
    }

    public async Task<object?> Handle(GetLogProcessamentoQuery r, CancellationToken ct)
    {
        return await _logService.GetLogByProcessamentoIdAsync(r.ProcessamentoId, r.EmpresaId, ct);
    }
}
