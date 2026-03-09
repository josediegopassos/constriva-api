using Constriva.Domain.Enums;
using Microsoft.AspNetCore.Http;

namespace Constriva.Application.Features.GED.DTOs;

public record UploadDocumentoDto(
    Guid PastaId, string Codigo, string Titulo, TipoDocumentoGEDEnum Tipo,
    string? Descricao, Guid? ObraId, IFormFile? Arquivo = null);
