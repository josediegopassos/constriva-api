using MediatR;
using Constriva.Application.Common.Behaviors;
using Constriva.Domain.Entities.Qualidade;
using Constriva.Domain.Enums;
using Constriva.Domain.Interfaces.Repositories;
using Constriva.Application.Features.Qualidade.DTOs;

namespace Constriva.Application.Features.Qualidade.Commands;

public record CreateInspecaoCommand(Guid EmpresaId, CreateInspecaoDto Dto)
    : IRequest<InspecaoDto>, ITenantRequest;

public class CreateInspecaoCommandHandler : IRequestHandler<CreateInspecaoCommand, InspecaoDto>
{
    private readonly IQualidadeRepository _repo;
    private readonly IUnitOfWork _uow;

    public CreateInspecaoCommandHandler(IQualidadeRepository repo, IUnitOfWork uow)
    {
        _repo = repo;
        _uow = uow;
    }

    public async Task<InspecaoDto> Handle(CreateInspecaoCommand request, CancellationToken cancellationToken)
    {
        var dto = request.Dto;
        var inspecao = new Inspecao
        {
            EmpresaId = request.EmpresaId,
            ObraId = dto.ObraId,
            Numero = dto.Numero,
            Titulo = dto.Titulo,
            Descricao = dto.Descricao,
            DataProgramada = dto.DataProgramada,
            Localicacao = dto.Localizacao,
            Status = StatusInspecaoEnum.Pendente,
            TemNaoConformidade = false
        };

        await _repo.AddInspecaoAsync(inspecao, cancellationToken);
        await _uow.SaveChangesAsync(cancellationToken);

        return new InspecaoDto(
            inspecao.Id, inspecao.ObraId, inspecao.Numero, inspecao.Titulo,
            inspecao.Descricao, inspecao.Status, inspecao.DataProgramada,
            null, inspecao.Localicacao, false, inspecao.CreatedAt);
    }
}
