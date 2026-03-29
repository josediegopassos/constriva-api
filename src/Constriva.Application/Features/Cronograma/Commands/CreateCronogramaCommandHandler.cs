using MediatR;
using Constriva.Application.Common.Behaviors;
using Constriva.Application.Common.Interfaces;
using Constriva.Domain.Entities.Cronograma;
using Constriva.Domain.Interfaces.Repositories;
using Constriva.Application.Features.Cronograma.DTOs;

namespace Constriva.Application.Features.Cronograma.Commands;

public record CreateCronogramaCommand(
    Guid ObraId,
    Guid EmpresaId,
    string Nome,
    DateTime DataInicio,
    DateTime DataFim,
    string? Descricao = null,
    string? Observacoes = null,
    bool ELinhaDBase = false,
    Guid? VersaoBaseadaEm = null)
    : IRequest<CronogramaObraDto>, ITenantRequest;

public class CreateCronogramaCommandHandler : IRequestHandler<CreateCronogramaCommand, CronogramaObraDto>
{
    private readonly ICronogramaRepository _repo;
    private readonly IUnitOfWork _uow;

    public CreateCronogramaCommandHandler(ICronogramaRepository repo, IUnitOfWork uow)
    {
        _repo = repo;
        _uow = uow;
    }

    public async Task<CronogramaObraDto> Handle(CreateCronogramaCommand request, CancellationToken cancellationToken)
    {
        var existente = await _repo.GetByObraAsync(request.ObraId, request.EmpresaId, cancellationToken);
        if (existente != null)
            throw new InvalidOperationException("Já existe um cronograma cadastrado para esta obra.");

        var cronograma = new CronogramaObra
        {
            ObraId          = request.ObraId,
            EmpresaId       = request.EmpresaId,
            Nome            = request.Nome,
            DataInicio      = request.DataInicio,
            DataFim         = request.DataFim,
            Descricao       = request.Descricao,
            Observacoes     = request.Observacoes,
            ELinhaDBase     = request.ELinhaDBase,
            VersaoBaseadaEm = request.VersaoBaseadaEm,
            Versao          = 1,
            Ativo           = true
        };

        await _repo.AddCronogramaAsync(cronograma, cancellationToken);
        await _uow.SaveChangesAsync(cancellationToken);

        return new CronogramaObraDto(
            cronograma.Id, cronograma.ObraId, cronograma.Nome, "",
            cronograma.DataInicio, cronograma.DataFim,
            0m, 0m, []);
    }
}
