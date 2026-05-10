namespace Constriva.Domain.ValueObjects.WhatsApp;

public class MediaInfoValueObject
{
    public required string WaMediaId { get; init; }
    public required string Url { get; init; }
    public required string MimeType { get; init; }
    public required long TamanhoBytes { get; init; }
    public string? NomeArquivo { get; init; }
}
