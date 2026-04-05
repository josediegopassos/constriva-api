using Microsoft.AspNetCore.Http;

namespace Constriva.Application.Features.Lens.Interfaces;

public interface ILensFileStorageService
{
    Task<string> SaveAsync(IFormFile arquivo, string subpasta, CancellationToken ct);
    Task<byte[]> ReadAsync(string caminho, CancellationToken ct);
    Task DeleteAsync(string caminho, CancellationToken ct);
    bool Exists(string caminho);
}
