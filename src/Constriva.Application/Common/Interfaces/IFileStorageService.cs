namespace Constriva.Application.Common.Interfaces;

public interface IFileStorageService
{
    Task<string> UploadAsync(Stream stream, string fileName, string contentType, string folder = "", CancellationToken ct = default);
    Task DeleteAsync(string url, CancellationToken ct = default);
    Task<Stream> DownloadAsync(string url, CancellationToken ct = default);
    string GetPublicUrl(string path);
}
