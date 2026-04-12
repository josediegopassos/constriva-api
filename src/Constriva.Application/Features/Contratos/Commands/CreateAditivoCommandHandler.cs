using MediatR;
using Constriva.Application.Common.Behaviors;
using Constriva.Domain.Entities.Contratos;
using Constriva.Domain.Interfaces.Repositories;
using Constriva.Application.Features.Contratos.DTOs;

namespace Constriva.Application.Features.Contratos.Commands;

public record CreateAditivoCommand(Guid ContratoId, Guid EmpresaId, CreateAditivoDto Dto)
    : IRequest<AditivoContratoDto>, ITenantRequest;

public class CreateAditivoCommandHandler : IRequestHandler<CreateAditivoCommand, AditivoContratoDto>
{
    private readonly IContratoRepository _repo;
    private readonly IUnitOfWork _uow;

    public CreateAditivoCommandHandler(IContratoRepository repo, IUnitOfWork uow)
    {
        _repo = repo;
        _uow = uow;
    }

    public async Task<AditivoContratoDto> Handle(CreateAditivoCommand request, CancellationToken cancellationToken)
    {
        var contrato = await _repo.GetByIdAndEmpresaAsync(request.ContratoId, request.EmpresaId, cancellationToken)
            ?? throw new KeyNotFoundException("Contrato não encontrado.");

        var count = await _repo.GetAditivosCountAsync(request.ContratoId, request.EmpresaId, cancellationToken);
        var numero = $"ADT-{(count + 1):D3}";
        var dto = request.Dto;

        var aditivo = new AditvoContrato
        {
            ContratoId = request.ContratoId,
            EmpresaId = request.EmpresaId,
            Numero = numero,
            Tipo = dto.Tipo,
            Justificativa = dto.Justificativa,
            DataAssinatura = dto.DataAssinatura,
            ValorAditivo = dto.ValorAditivo,
            ProrrogacaoDias = dto.ProrrogacaoDias,
            NovaDataVigencia = dto.NovaDataVigencia,
            ArquivoUrl = dto.ArquivoUrl
        };

        await _repo.AddAditivoAsync(aditivo, cancellationToken);

        contrato.ValorAditivos += dto.ValorAditivo;
        if (dto.NovaDataVigencia.HasValue)
            contrato.DataVigenciaFim = dto.NovaDataVigencia.Value;

        _repo.Update(contrato);
        await _uow.SaveChangesAsync(cancellationToken);

        return new AditivoContratoDto(
            aditivo.Id, aditivo.ContratoId, aditivo.Numero, aditivo.Tipo,
            aditivo.Justificativa, aditivo.DataAssinatura, aditivo.ValorAditivo,
            aditivo.ProrrogacaoDias, aditivo.NovaDataVigencia, aditivo.ArquivoUrl,
            aditivo.CreatedAt);
    }
}
