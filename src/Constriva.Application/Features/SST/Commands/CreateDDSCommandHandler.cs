using MediatR;
using Constriva.Application.Common.Behaviors;
using Constriva.Domain.Entities.SST;
using Constriva.Domain.Enums;
using Constriva.Domain.Interfaces.Repositories;
using Constriva.Application.Features.SST.DTOs;

namespace Constriva.Application.Features.SST.Commands;

public record CreateDDSCommand(Guid EmpresaId, Guid UsuarioId, CreateDDSDto Dto)
    : IRequest<DDSDto>, ITenantRequest;

public class CreateDDSCommandHandler : IRequestHandler<CreateDDSCommand, DDSDto>
{
    private readonly ISSTRepository _repo;
    private readonly IUnitOfWork _uow;

    public CreateDDSCommandHandler(ISSTRepository repo, IUnitOfWork uow)
    {
        _repo = repo;
        _uow = uow;
    }

    public async Task<DDSDto> Handle(CreateDDSCommand request, CancellationToken cancellationToken)
    {
        var dto = request.Dto;
        var dds = new DDS
        {
            EmpresaId = request.EmpresaId,
            ObraId = dto.ObraId,
            Numero = dto.Numero,
            Tema = dto.Tema,
            Conteudo = dto.Conteudo,
            Ministrador = dto.Ministrador,
            NumeroParticipantes = dto.NumeroParticipantes,
            DataRealizacao = dto.DataRealizacao,
            RegistradoPor = request.UsuarioId
        };

        await _repo.AddDDSAsync(dds, cancellationToken);
        await _uow.SaveChangesAsync(cancellationToken);

        return new DDSDto(
            dds.Id, dds.ObraId, dds.Numero, dds.Tema, dds.Conteudo,
            dds.Ministrador, dds.NumeroParticipantes, dds.DataRealizacao, dds.CreatedAt);
    }
}
