using FluentValidation;
using MediatR;
using Constriva.Application.Common.Behaviors;
using Constriva.Domain.Entities.Qualidade;
using Constriva.Domain.Enums;
using Constriva.Domain.Interfaces.Repositories;
using Constriva.Application.Features.Qualidade.DTOs;

namespace Constriva.Application.Features.Qualidade.Commands;

public record UpdateNCCommand(Guid Id, Guid EmpresaId, string Descricao, string? Causa,
    string? AcaoCorretiva, DateTime? DataPrazo, string Status)
    : IRequest<NaoConformidadeDto>, ITenantRequest { public Guid TenantId => EmpresaId; }

public class UpdateNCCommandHandler : IRequestHandler<UpdateNCCommand, NaoConformidadeDto>
{
    private readonly IQualidadeRepository _repo;
    private readonly IUnitOfWork _uow;

    public UpdateNCCommandHandler(IQualidadeRepository repo, IUnitOfWork uow)
    {
        _repo = repo;
        _uow = uow;
    }

    public async Task<NaoConformidadeDto> Handle(UpdateNCCommand request, CancellationToken cancellationToken)
    {
        var nc = await _repo.GetNCByIdAsync(request.Id, request.EmpresaId, cancellationToken)
            ?? throw new KeyNotFoundException($"NC {request.Id} não encontrada.");

        nc.Descricao = request.Descricao;
        if (request.Causa != null) nc.CausaRaiz = request.Causa;
        if (request.AcaoCorretiva != null) nc.AcaoCorretiva = request.AcaoCorretiva;
        if (request.DataPrazo.HasValue) nc.DataPrazoConclusao = request.DataPrazo;
        if (Enum.TryParse<StatusNaoConformidadeEnum>(request.Status, out var newStatus))
        {
            if (newStatus == StatusNaoConformidadeEnum.Encerrada)
            {
                var acaoFinal = request.AcaoCorretiva ?? nc.AcaoCorretiva;
                if (string.IsNullOrWhiteSpace(acaoFinal))
                    throw new InvalidOperationException(
                        "Ação corretiva é obrigatória para encerrar uma não conformidade.");
                if (nc.DataEncerramento == null)
                    nc.DataEncerramento = DateTime.UtcNow;
            }
            nc.Status = newStatus;
        }

        await _uow.SaveChangesAsync(cancellationToken);

        return new NaoConformidadeDto(
            nc.Id, nc.ObraId, nc.Numero, nc.Titulo, nc.Descricao,
            nc.Status, nc.Gravidade, nc.LocalizacaoObra, nc.CausaRaiz, nc.AcaoCorretiva,
            nc.DataAbertura, nc.DataPrazoConclusao, nc.DataEncerramento);
    }
}
