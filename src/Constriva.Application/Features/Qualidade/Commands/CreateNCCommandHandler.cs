using MediatR;
using Constriva.Application.Common.Behaviors;
using Constriva.Application.Common.Interfaces;
using Constriva.Domain.Entities.Qualidade;
using Constriva.Domain.Enums;
using Constriva.Domain.Interfaces.Repositories;
using Constriva.Application.Features.Qualidade.DTOs;

namespace Constriva.Application.Features.Qualidade.Commands;

public record CreateNCCommand(Guid EmpresaId, CreateNCDto Dto)
    : IRequest<NaoConformidadeDto>, ITenantRequest;

public class CreateNCCommandHandler : IRequestHandler<CreateNCCommand, NaoConformidadeDto>
{
    private readonly IQualidadeRepository _repo;
    private readonly IUnitOfWork _uow;

    public CreateNCCommandHandler(IQualidadeRepository repo, IUnitOfWork uow)
    {
        _repo = repo;
        _uow = uow;
    }

    public async Task<NaoConformidadeDto> Handle(CreateNCCommand request, CancellationToken cancellationToken)
    {
        var dto = request.Dto;

        // Valida vínculo de inspeção quando informado, garantindo pertença ao tenant
        if (dto.InspecaoId.HasValue)
            _ = await _repo.GetInspecaoByIdAsync(dto.InspecaoId.Value, request.EmpresaId, cancellationToken)
                ?? throw new KeyNotFoundException($"Inspeção {dto.InspecaoId} não encontrada.");

        var nc = new NaoConformidade
        {
            EmpresaId = request.EmpresaId,
            ObraId = dto.ObraId,
            InspecaoId = dto.InspecaoId,
            Numero = dto.Numero,
            Titulo = dto.Titulo,
            Descricao = dto.Descricao,
            Gravidade = dto.Gravidade,
            LocalizacaoObra = dto.LocalizacaoObra,
            DataAbertura = DateTime.UtcNow,
            DataPrazoConclusao = dto.DataPrazoConclusao,
            ResponsavelId = dto.ResponsavelId,
            Status = StatusNaoConformidadeEnum.Aberta
        };

        await _repo.AddNCAsync(nc, cancellationToken);
        await _uow.SaveChangesAsync(cancellationToken);

        return new NaoConformidadeDto(
            nc.Id, nc.ObraId, nc.Numero, nc.Titulo, nc.Descricao,
            nc.Status, nc.Gravidade, nc.LocalizacaoObra, nc.CausaRaiz, nc.AcaoCorretiva,
            nc.DataAbertura, nc.DataPrazoConclusao, nc.DataEncerramento);
    }
}
