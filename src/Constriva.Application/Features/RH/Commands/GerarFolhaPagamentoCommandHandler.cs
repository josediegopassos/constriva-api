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

        var folhaFunc = request.FuncionarioId.HasValue
            ? folha.Funcionarios.FirstOrDefault(f => f.FuncionarioId == request.FuncionarioId)
            : folha.Funcionarios.FirstOrDefault();

        if (folhaFunc != null)
            return new FolhaPagamentoDto(
                folha.Id, folha.Competencia, folhaFunc.FuncionarioId, "",
                folhaFunc.SalarioBruto, folhaFunc.TotalProventos - folhaFunc.SalarioBruto,
                folhaFunc.TotalDescontos, folhaFunc.SalarioLiquido);

        return new FolhaPagamentoDto(
            folha.Id, folha.Competencia, Guid.Empty, "Total",
            folha.ValorTotalBruto, 0, folha.ValorTotalDescontos, folha.ValorTotalLiquido);
    }
}
