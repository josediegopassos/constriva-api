using MediatR;

namespace Constriva.Application.Common.Interfaces;


public interface ICurrentUser
{
    Guid UserId { get; }
    Guid? EmpresaId { get; }
    string Email { get; }
    bool IsSuperAdmin { get; }
    bool IsAdminEmpresa { get; }
    string Perfil { get; }
    bool HasPermission(string modulo, string acao);
}

public interface IEmailService
{
    Task SendAsync(string to, string subject, string body, CancellationToken ct = default);
    Task SendTemplateAsync(string to, string template, object model, CancellationToken ct = default);
}

public interface IFileStorageService
{
    Task<string> UploadAsync(Stream stream, string fileName, string contentType, string folder = "", CancellationToken ct = default);
    Task DeleteAsync(string url, CancellationToken ct = default);
    Task<Stream> DownloadAsync(string url, CancellationToken ct = default);
    string GetPublicUrl(string path);
}

public interface INotificationService
{
    Task SendPushAsync(Guid usuarioId, string title, string body, object? data = null, CancellationToken ct = default);
    Task SendToEmpresaAsync(Guid empresaId, string title, string body, object? data = null, CancellationToken ct = default);
}

public interface ICacheService
{
    Task<T?> GetAsync<T>(string key, CancellationToken ct = default);
    Task SetAsync<T>(string key, T value, TimeSpan? expiry = null, CancellationToken ct = default);
    Task RemoveAsync(string key, CancellationToken ct = default);
    Task RemoveByPatternAsync(string pattern, CancellationToken ct = default);
}

// Base result
public class Result
{
    public bool Success { get; protected set; }
    public string? Error { get; protected set; }
    public static Result Ok() => new() { Success = true };
    public static Result Fail(string error) => new() { Success = false, Error = error };
}

public class Result<T> : Result
{
    public T? Data { get; private set; }
    public static Result<T> Ok(T data) => new() { Success = true, Data = data };
    public new static Result<T> Fail(string error) => new() { Success = false, Error = error };
}

// Pagination
public class PaginatedQuery<T> : IRequest<PaginatedResult<T>>
{
    public int Page { get; init; } = 1;
    public int PageSize { get; init; } = 20;
    public string? Search { get; init; }
    public string? OrderBy { get; init; }
    public bool OrderDesc { get; init; } = false;
}

public class PaginatedResult<T>
{
    public IEnumerable<T> Items { get; init; } = Enumerable.Empty<T>();
    public int TotalCount { get; init; }
    public int Page { get; init; }
    public int PageSize { get; init; }
    public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
    public bool HasNext => Page < TotalPages;
    public bool HasPrevious => Page > 1;
}
