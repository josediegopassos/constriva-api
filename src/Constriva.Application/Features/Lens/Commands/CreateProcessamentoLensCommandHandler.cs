using MediatR;
using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Constriva.Application.Common.Behaviors;
using Constriva.Application.Features.Lens.DTOs;
using Constriva.Application.Features.Lens.Interfaces;
using Constriva.Domain.Entities.Lens;
using Constriva.Domain.Enums;
using Constriva.Domain.Interfaces.Repositories;
using Constriva.Application.Features.Lens.Extensions;
using Constriva.Messaging.Contracts.Lens.Commands;

namespace Constriva.Application.Features.Lens.Commands;

public record CreateProcessamentoLensCommand(
    Guid EmpresaId,
    Guid UsuarioId,
    IFormFile Arquivo,
    InitProcessamentoLensDto Dto) : IRequest<Guid>, ITenantRequest;

public class CreateProcessamentoLensCommandHandler : IRequestHandler<CreateProcessamentoLensCommand, Guid>
{
    private readonly ITenantRepository<DocumentoLens> _repo;
    private readonly ILensFileStorageService _fileStorage;
    private readonly IUnitOfWork _uow;
    private readonly IBus _bus;
    private readonly ILogger<CreateProcessamentoLensCommandHandler> _logger;

    public CreateProcessamentoLensCommandHandler(
        ITenantRepository<DocumentoLens> repo,
        ILensFileStorageService fileStorage,
        IUnitOfWork uow,
        IBus bus,
        ILogger<CreateProcessamentoLensCommandHandler> logger)
    {
        _repo = repo;
        _fileStorage = fileStorage;
        _uow = uow;
        _bus = bus;
        _logger = logger;
    }

    public async Task<Guid> Handle(CreateProcessamentoLensCommand r, CancellationToken ct)
    {
        var arquivo = r.Arquivo;
        var extensao = Path.GetExtension(arquivo.FileName).ToLowerInvariant();

        var caminho = await _fileStorage.SaveAsync(arquivo, $"lens/{r.EmpresaId}", ct);

        var documento = new DocumentoLens
        {
            EmpresaId = r.EmpresaId,
            UsuarioId = r.UsuarioId,
            ObraId = r.Dto.ObraId,
            CentroCustoId = r.Dto.CentroCustoId,
            TipoDocumento = r.Dto.TipoDocumento,
            TipoDocumentoDeclarado = r.Dto.TipoDocumento,
            Status = StatusProcessamentoLensEnum.Pendente,
            NomeArquivo = arquivo.FileName,
            CaminhoArquivo = caminho,
            ExtensaoArquivo = extensao,
            TamanhoBytes = arquivo.Length,
            Observacoes = r.Dto.Observacoes
        };

        await _repo.AddAsync(documento, ct);
        await _uow.SaveChangesAsync(ct);

        try
        {
            using var publishCts = CancellationTokenSource.CreateLinkedTokenSource(ct);
            publishCts.CancelAfter(TimeSpan.FromSeconds(10));

            await _bus.Publish(new ProcessDocumentoLensCommand
            {
                ProcessamentoId = documento.Id,
                UsuarioId = r.UsuarioId,
                EmpresaId = r.EmpresaId,
                ObraId = r.Dto.ObraId,
                CentroCustoId = r.Dto.CentroCustoId,
                TipoDocumento = r.Dto.TipoDocumento.ToLensApiString(),
                NomeArquivo = arquivo.FileName,
                CaminhoArquivo = caminho,
                ExtensaoArquivo = extensao,
                TamanhoBytes = arquivo.Length,
                Observacoes = r.Dto.Observacoes
            }, publishCts.Token);
        }
        catch (Exception ex) when (ex is OperationCanceledException or TimeoutException)
        {
            _logger.LogWarning("RabbitMQ indisponível. Processamento {Id} salvo com status Pendente para reprocessamento posterior.", documento.Id);
        }

        return documento.Id;
    }
}
