using Microsoft.AspNetCore.Http;
using Constriva.Application.Features.Lens.Interfaces;

namespace Constriva.API.Services;

public class LensFileStorageService : ILensFileStorageService
{
    private readonly string _diretorioBase;
    private readonly ILogger<LensFileStorageService> _logger;
    private static readonly HashSet<string> _extensoesPermitidas = new(StringComparer.OrdinalIgnoreCase)
    {
        ".pdf", ".jpg", ".jpeg", ".png", ".tiff", ".tif", ".bmp", ".webp"
    };
    private const long TamanhoMaximoBytes = 50 * 1024 * 1024; // 50 MB

    public LensFileStorageService(IConfiguration configuration, ILogger<LensFileStorageService> logger)
    {
        var configDir = configuration["Lens:DiretorioUploads"];
        _diretorioBase = string.IsNullOrWhiteSpace(configDir)
            ? Path.Combine(AppContext.BaseDirectory, "uploads", "lens")
            : configDir;
        _logger = logger;
        if (!Directory.Exists(_diretorioBase))
            Directory.CreateDirectory(_diretorioBase);
    }

    public async Task<string> SaveAsync(IFormFile arquivo, string subpasta, CancellationToken ct)
    {
        if (arquivo.Length == 0)
            throw new ArgumentException("O arquivo enviado está vazio.");
        if (arquivo.Length > TamanhoMaximoBytes)
            throw new ArgumentException($"O arquivo excede o tamanho máximo permitido de {TamanhoMaximoBytes / (1024 * 1024)} MB.");

        var extensao = Path.GetExtension(arquivo.FileName).ToLowerInvariant();
        if (!_extensoesPermitidas.Contains(extensao))
            throw new ArgumentException($"Extensão '{extensao}' não é permitida. Extensões aceitas: {string.Join(", ", _extensoesPermitidas)}");

        var agora = DateTime.UtcNow;
        var diretorio = Path.Combine(_diretorioBase, agora.Year.ToString(), agora.Month.ToString("D2"), subpasta);
        if (!Directory.Exists(diretorio))
            Directory.CreateDirectory(diretorio);

        var nomeArquivo = $"{Guid.NewGuid()}{extensao}";
        var caminhoCompleto = Path.Combine(diretorio, nomeArquivo);

        await using var stream = new FileStream(caminhoCompleto, FileMode.Create);
        await arquivo.CopyToAsync(stream, ct);

        _logger.LogInformation("Arquivo Lens salvo em {Caminho}. Tamanho: {Tamanho} bytes.", caminhoCompleto, arquivo.Length);
        return caminhoCompleto;
    }

    public async Task<byte[]> ReadAsync(string caminho, CancellationToken ct)
    {
        if (!File.Exists(caminho))
            throw new FileNotFoundException($"Arquivo não encontrado: {caminho}");
        return await File.ReadAllBytesAsync(caminho, ct);
    }

    public Task DeleteAsync(string caminho, CancellationToken ct)
    {
        if (File.Exists(caminho))
        {
            File.Delete(caminho);
            _logger.LogInformation("Arquivo Lens excluído: {Caminho}.", caminho);
        }
        return Task.CompletedTask;
    }

    public bool Exists(string caminho) => File.Exists(caminho);
}
