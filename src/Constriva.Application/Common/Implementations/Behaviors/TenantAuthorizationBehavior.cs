using Constriva.Application.Common.Interfaces;
using MediatR;

namespace Constriva.Application.Common.Implementations.Behaviors;

public class TenantAuthorizationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : ITenantRequest
{
    private readonly ICurrentUser _currentUser;

    public TenantAuthorizationBehavior(ICurrentUser currentUser)
    {
        _currentUser = currentUser;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken ct)
    {
        if (!_currentUser.IsSuperAdmin && _currentUser.EmpresaId != request.EmpresaId)
            throw new UnauthorizedAccessException("Acesso negado para esta empresa.");
        return await next();
    }
}
