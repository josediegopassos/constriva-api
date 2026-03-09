using Constriva.Domain.Enums;

namespace Constriva.Application.Features.GED.DTOs;

public record DocumentoDto(Guid Id, string Titulo, TipoDocumentoGEDEnum Tipo, Guid PastaId, DateTime? DataVigencia, string? Palavraschave, DateTime CreatedAt);
