using FluentValidation;
using MediatR;
using Constriva.Application.Common.Behaviors;
using Constriva.Application.Common.Interfaces;
using Constriva.Domain.Entities.GED;
using Constriva.Domain.Enums;
using Constriva.Domain.Interfaces.Repositories;
using Microsoft.AspNetCore.Http;
using Constriva.Application.Features.GED.DTOs;

namespace Constriva.Application.Features.GED.Commands;

public record UploadDocumentoCommand(Guid EmpresaId, UploadDocumentoDto Dto, Guid UsuarioId = default)
    : IRequest<DocumentoGEDDto>, ITenantRequest;

public class UploadDocumentoCommandHandler : IRequestHandler<UploadDocumentoCommand, DocumentoGEDDto>
{
    private const long MaxFileSizeBytes = 50 * 1024 * 1024; // 50 MB
    private static readonly HashSet<string> TiposPermitidos = new(StringComparer.OrdinalIgnoreCase)
    {
        "application/pdf", "image/jpeg", "image/png", "image/gif",
        "application/msword", "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
        "application/vnd.ms-excel", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
        "application/zip", "text/plain"
    };

    private readonly IGEDRepository _repo;
    private readonly IFileStorageService _storage;
    private readonly IUnitOfWork _uow;

    public UploadDocumentoCommandHandler(IGEDRepository repo, IFileStorageService storage, IUnitOfWork uow)
    {
        _repo = repo;
        _storage = storage;
        _uow = uow;
    }

    public async Task<DocumentoGEDDto> Handle(UploadDocumentoCommand request, CancellationToken cancellationToken)
    {
        var dto = request.Dto;
        string? url = null;
        long tamanhoBytes = 0;

        if (dto.Arquivo != null)
        {
            if (dto.Arquivo.Length > MaxFileSizeBytes)
                throw new InvalidOperationException(
                    $"Arquivo excede o tamanho máximo permitido de {MaxFileSizeBytes / (1024 * 1024)} MB.");

            if (!TiposPermitidos.Contains(dto.Arquivo.ContentType))
                throw new InvalidOperationException(
                    $"Tipo de arquivo '{dto.Arquivo.ContentType}' não é permitido.");

            tamanhoBytes = dto.Arquivo.Length;
            using var stream = dto.Arquivo.OpenReadStream();
            url = await _storage.UploadAsync(
                stream, dto.Arquivo.FileName, dto.Arquivo.ContentType,
                $"ged/{request.EmpresaId}", cancellationToken);
        }

        var documento = new DocumentoGED
        {
            EmpresaId = request.EmpresaId,
            PastaId = dto.PastaId,
            Codigo = dto.Codigo,
            Titulo = dto.Titulo,
            Descricao = dto.Descricao,
            Tipo = dto.Tipo,
            ObraId = dto.ObraId,
            Status = StatusDocumentoGEDEnum.Rascunho,
            Versao = "1"
        };

        // Registra o arquivo físico com rastreabilidade de quem fez o upload
        if (url != null)
        {
            documento.Arquivos.Add(new ArquivoDocumento
            {
                EmpresaId = request.EmpresaId,
                NomeArquivo = dto.Arquivo!.FileName,
                TipoArquivo = dto.Arquivo.ContentType,
                Url = url,
                TamanhoBytes = tamanhoBytes,
                NumeroRevisao = 0,
                Atual = true,
                UploadPor = request.UsuarioId
            });
        }

        await _repo.AddDocumentoAsync(documento, cancellationToken);
        await _uow.SaveChangesAsync(cancellationToken);

        return new DocumentoGEDDto(
            documento.Id, documento.PastaId, documento.Codigo, documento.Titulo,
            documento.Descricao, documento.Tipo, documento.Status,
            documento.Versao, 0, null, false, documento.CreatedAt,
            FileUrl: url, FileSizeBytes: tamanhoBytes);
    }
}
