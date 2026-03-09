using Constriva.Domain.Enums;

namespace Constriva.Application.Features.GED.DTOs;

public record DocumentoGEDDto(
    Guid Id, Guid PastaId, string Codigo, string Titulo, string? Descricao,
    TipoDocumentoGEDEnum Tipo, StatusDocumentoGEDEnum Status,
    string? Versao, int NumeroRevisao, DateTime? DataVigencia,
    bool Aprovado, DateTime CreatedAt, string? FileUrl = null, long FileSizeBytes = 0);
