using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using Constriva.Application.Common.Interfaces;
using System.Diagnostics;

namespace Constriva.Application.Common.Behaviors;

public interface ITenantRequest { Guid EmpresaId { get; } }

// Validation behavior
public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators) => _validators = validators;

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken ct)
    {
        if (!_validators.Any()) return await next();

        var context = new ValidationContext<TRequest>(request);
        var failures = _validators
            .Select(v => v.Validate(context))
            .SelectMany(r => r.Errors)
            .Where(f => f != null)
            .ToList();

        if (failures.Count != 0)
            throw new ValidationException(failures);

        return await next();
    }
}

// Logging behavior
public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger;

    public LoggingBehavior(ILogger<LoggingBehavior<TRequest, TResponse>> logger) => _logger = logger;

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken ct)
    {
        var name = typeof(TRequest).Name;
        _logger.LogInformation("Handling {RequestName}", name);
        var sw = Stopwatch.StartNew();
        var response = await next();
        sw.Stop();
        if (sw.ElapsedMilliseconds > 500)
            _logger.LogWarning("Long running request {RequestName} ({ElapsedMs}ms)", name, sw.ElapsedMilliseconds);
        return response;
    }
}

// Tenant authorization behavior
public class TenantAuthorizationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : ITenantRequest
{
    private readonly ICurrentUser _currentUser;

    public TenantAuthorizationBehavior(ICurrentUser currentUser) => _currentUser = currentUser;

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken ct)
    {
        if (!_currentUser.IsSuperAdmin && _currentUser.EmpresaId != request.EmpresaId)
            throw new UnauthorizedAccessException("Acesso negado para esta empresa.");
        return await next();
    }
}

