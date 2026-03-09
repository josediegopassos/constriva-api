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

public record UpdateDocumentoCommand(Guid Id, Guid EmpresaId, string Nome, string? Descricao,
    DateTime? DataVencimento, string? Tags)
    : IRequest<DocumentoDto>, ITenantRequest { public Guid TenantId => EmpresaId; }

public class UpdateDocumentoHandler : IRequestHandler<UpdateDocumentoCommand, DocumentoDto>
{
    private readonly IGEDRepository _repo;
    private readonly IUnitOfWork _uow;

    public UpdateDocumentoHandler(IGEDRepository repo, IUnitOfWork uow)
    {
        _repo = repo;
        _uow = uow;
    }

    public async Task<DocumentoDto> Handle(UpdateDocumentoCommand request, CancellationToken cancellationToken)
    {
        var doc = await _repo.GetDocumentoByIdAsync(request.Id, request.EmpresaId, cancellationToken)
            ?? throw new KeyNotFoundException($"Documento {request.Id} não encontrado.");

        doc.Titulo = request.Nome;
        if (request.Descricao != null) doc.Descricao = request.Descricao;
        if (request.DataVencimento.HasValue) doc.DataVigencia = request.DataVencimento;
        if (request.Tags != null)
        {
            var tags = string.Join(",", request.Tags.Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Select(t => t.Trim())
                .Where(t => t.Length > 0)
                .Distinct(StringComparer.OrdinalIgnoreCase));
            doc.Palavraschave = tags.Length > 500 ? tags[..500] : tags;
        }

        await _uow.SaveChangesAsync(cancellationToken);

        return new DocumentoDto(doc.Id, doc.Titulo, doc.Tipo, doc.PastaId, doc.DataVigencia, doc.Palavraschave, doc.CreatedAt);
    }
}
