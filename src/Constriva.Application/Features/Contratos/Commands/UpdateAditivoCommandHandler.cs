using MediatR;
using Constriva.Application.Common.Behaviors;
using Constriva.Domain.Interfaces.Repositories;
using Constriva.Application.Features.Contratos.DTOs;

namespace Constriva.Application.Features.Contratos.Commands;

public record UpdateAditivoCommand(Guid Id, Guid ContratoId, Guid EmpresaId, UpdateAditivoDto Dto)
    : IRequest<AditivoContratoDto>, ITenantRequest { public Guid TenantId => EmpresaId; }

public class UpdateAditivoHandler : IRequestHandler<UpdateAditivoCommand, AditivoContratoDto>
{
    private readonly IContratoRepository _repo;
    private readonly IUnitOfWork _uow;

    public UpdateAditivoHandler(IContratoRepository repo, IUnitOfWork uow)
    {
        _repo = repo;
        _uow = uow;
    }

    public async Task<AditivoContratoDto> Handle(UpdateAditivoCommand request, CancellationToken cancellationToken)
    {
        var aditivo = await _repo.GetAditivoByIdAsync(request.Id, request.EmpresaId, cancellationToken)
            ?? throw new KeyNotFoundException($"Aditivo {request.Id} não encontrado.");

        var contrato = await _repo.GetByIdAndEmpresaAsync(request.ContratoId, request.EmpresaId, cancellationToken)
            ?? throw new KeyNotFoundException("Contrato não encontrado.");

        var dto = request.Dto;

        // Ajustar ValorAditivos do contrato (remover antigo, somar novo)
        contrato.ValorAditivos = contrato.ValorAditivos - aditivo.ValorAditivo + dto.ValorAditivo;

        aditivo.Tipo = dto.Tipo;
        aditivo.Justificativa = dto.Justificativa;
        aditivo.DataAssinatura = dto.DataAssinatura;
        aditivo.ValorAditivo = dto.ValorAditivo;
        aditivo.ProrrogacaoDias = dto.ProrrogacaoDias;
        aditivo.NovaDataVigencia = dto.NovaDataVigencia;
        aditivo.ArquivoUrl = dto.ArquivoUrl;

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
