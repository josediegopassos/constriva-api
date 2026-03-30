using FluentValidation;
using MediatR;
using Constriva.Application.Common.Behaviors;
using Constriva.Domain.Entities.RH;
using Constriva.Domain.Enums;
using Constriva.Domain.Interfaces.Repositories;
using Constriva.Application.Features.RH.DTOs;

namespace Constriva.Application.Features.RH.Commands;

public record GerarFolhaPagamentoCommand(Guid EmpresaId, string Competencia, Guid? FuncionarioId)
    : IRequest<FolhaPagamentoDto>, ITenantRequest { public Guid TenantId => EmpresaId; }

public class GerarFolhaPagamentoCommandHandler : IRequestHandler<GerarFolhaPagamentoCommand, FolhaPagamentoDto>
{
    private readonly IRHRepository _repo;
    private readonly IUnitOfWork _uow;

    public GerarFolhaPagamentoCommandHandler(IRHRepository repo, IUnitOfWork uow)
    {
        _repo = repo;
        _uow = uow;
    }

    public async Task<FolhaPagamentoDto> Handle(GerarFolhaPagamentoCommand request, CancellationToken cancellationToken)
    {
        if (!System.Text.RegularExpressions.Regex.IsMatch(request.Competencia, @"^\d{4}-(0[1-9]|1[0-2])$"))
            throw new ArgumentException(
                $"Competência '{request.Competencia}' inválida. Use o formato AAAA-MM (ex: 2025-03).");

        var folha = await _repo.GerarFolhaAsync(request.EmpresaId, request.Competencia, request.FuncionarioId, cancellationToken);
        await _uow.SaveChangesAsync(cancellationToken);

        return new FolhaPagamentoDto(
            folha.Id, folha.Competencia, folha.DataInicio, folha.DataFim,
            folha.Status, folha.TotalFuncionarios,
            folha.ValorTotalBruto, folha.ValorTotalDescontos, folha.ValorTotalLiquido,
            folha.AprovadoPor, folha.DataAprovacao, folha.DataPagamento);
    }
}
