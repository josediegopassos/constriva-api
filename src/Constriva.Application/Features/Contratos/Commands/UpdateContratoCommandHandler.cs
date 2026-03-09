using FluentValidation;
using MediatR;
using Constriva.Application.Common.Behaviors;
using Constriva.Domain.Entities.Contratos;
using Constriva.Domain.Enums;
using Constriva.Domain.Interfaces.Repositories;
using Constriva.Application.Features.Contratos.DTOs;

namespace Constriva.Application.Features.Contratos.Commands;

public record UpdateContratoCommand(Guid Id, Guid EmpresaId, string Numero, string Objeto,
    decimal ValorTotal, DateTime DataInicio, DateTime DataFim, string Status, string? Observacoes)
    : IRequest<ContratoDto>, ITenantRequest { public Guid TenantId => EmpresaId; }

public class UpdateContratoHandler : IRequestHandler<UpdateContratoCommand, ContratoDto>
{
    private readonly IContratoRepository _repo;
    private readonly IUnitOfWork _uow;

    public UpdateContratoHandler(IContratoRepository repo, IUnitOfWork uow)
    {
        _repo = repo;
        _uow = uow;
    }

    public async Task<ContratoDto> Handle(UpdateContratoCommand request, CancellationToken cancellationToken)
    {
        var contrato = await _repo.GetByIdAndEmpresaAsync(request.Id, request.EmpresaId, cancellationToken)
            ?? throw new KeyNotFoundException($"Contrato {request.Id} não encontrado.");

        if (request.DataFim <= request.DataInicio)
            throw new InvalidOperationException("A data de fim deve ser posterior à data de início do contrato.");

        contrato.Objeto = request.Objeto;
        contrato.ValorGlobal = request.ValorTotal;
        contrato.DataVigenciaInicio = request.DataInicio;
        contrato.DataVigenciaFim = request.DataFim;
        contrato.Observacoes = request.Observacoes;
        if (!Enum.TryParse<StatusContratoEnum>(request.Status, ignoreCase: true, out var newStatus))
            throw new InvalidOperationException(
                $"Status inválido: '{request.Status}'. Valores aceitos: {string.Join(", ", Enum.GetNames<StatusContratoEnum>())}.");
        contrato.Status = newStatus;

        _repo.Update(contrato);
        await _uow.SaveChangesAsync(cancellationToken);

        return new ContratoDto(
            contrato.Id, contrato.Numero, contrato.Objeto, contrato.Tipo, contrato.Status,
            contrato.ObraId, contrato.FornecedorId, null,
            contrato.ValorGlobal, contrato.ValorMedidoAcumulado, 0,
            contrato.DataAssinatura, contrato.DataVigenciaInicio, contrato.DataVigenciaFim,
            contrato.Observacoes, contrato.CreatedAt);
    }
}
