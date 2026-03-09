using MediatR;
using Constriva.Application.Common.Behaviors;
using Constriva.Application.Features.Financeiro;
using Constriva.Domain.Entities.Financeiro;
using Constriva.Domain.Enums;
using Constriva.Domain.Interfaces.Repositories;
using Constriva.Application.Features.Financeiro.DTOs;

namespace Constriva.Application.Features.Financeiro.Commands;

public record UpdateLancamentoCommand(Guid Id, Guid EmpresaId, UpdateLancamentoDto Dto)
    : IRequest<Unit>, ITenantRequest { public Guid TenantId => EmpresaId; }

public class UpdateLancamentoHandler : IRequestHandler<UpdateLancamentoCommand, Unit>
{
    private readonly ILancamentoFinanceiroRepository _repo;
    private readonly IObraRepository _obraRepo;
    private readonly IFornecedorRepository _fornecedorRepo;
    private readonly ICentroCustoRepository _centroCustoRepo;
    private readonly IContaBancariaRepository _contaBancariaRepo;
    private readonly IUnitOfWork _uow;

    public UpdateLancamentoHandler(
        ILancamentoFinanceiroRepository repo,
        IObraRepository obraRepo,
        IFornecedorRepository fornecedorRepo,
        ICentroCustoRepository centroCustoRepo,
        IContaBancariaRepository contaBancariaRepo,
        IUnitOfWork uow)
    {
        _repo = repo;
        _obraRepo = obraRepo;
        _fornecedorRepo = fornecedorRepo;
        _centroCustoRepo = centroCustoRepo;
        _contaBancariaRepo = contaBancariaRepo;
        _uow = uow;
    }

    public async Task<Unit> Handle(UpdateLancamentoCommand request, CancellationToken cancellationToken)
    {
        var lancamento = await _repo.GetByIdAndEmpresaAsync(request.Id, request.EmpresaId, cancellationToken)
            ?? throw new KeyNotFoundException($"Lançamento {request.Id} não encontrado.");

        if (lancamento.Status == StatusLancamentoEnum.Realizado)
            throw new InvalidOperationException("Lançamento já realizado não pode ser alterado.");

        var dto = request.Dto;

        // Valida FKs quando alteradas para um novo valor não-nulo
        if (dto.ObraId.HasValue)
            _ = await _obraRepo.GetByIdAndEmpresaAsync(dto.ObraId.Value, request.EmpresaId, cancellationToken)
                ?? throw new KeyNotFoundException($"Obra {dto.ObraId} não encontrada.");

        if (dto.FornecedorId.HasValue)
            _ = await _fornecedorRepo.GetByIdAndEmpresaAsync(dto.FornecedorId.Value, request.EmpresaId, cancellationToken)
                ?? throw new KeyNotFoundException($"Fornecedor {dto.FornecedorId} não encontrado.");

        if (dto.CentroCustoId.HasValue)
            _ = await _centroCustoRepo.GetByIdAndEmpresaAsync(dto.CentroCustoId.Value, request.EmpresaId, cancellationToken)
                ?? throw new KeyNotFoundException($"Centro de custo {dto.CentroCustoId} não encontrado.");

        if (dto.Descricao != null) lancamento.Descricao = dto.Descricao;
        if (dto.Valor.HasValue) lancamento.Valor = dto.Valor.Value;
        if (dto.DataVencimento.HasValue) lancamento.DataVencimento = dto.DataVencimento.Value;
        if (dto.ObraId.HasValue) lancamento.ObraId = dto.ObraId;
        if (dto.FornecedorId.HasValue) lancamento.FornecedorId = dto.FornecedorId;
        if (dto.CentroCustoId.HasValue) lancamento.CentroCustoId = dto.CentroCustoId;
        if (dto.FormaPagamentoEnum.HasValue) lancamento.FormaPagamentoEnum = dto.FormaPagamentoEnum;
        if (dto.NumeroDocumento != null) lancamento.NumeroDocumento = dto.NumeroDocumento;
        if (dto.NumeroNF != null) lancamento.NumeroNF = dto.NumeroNF;
        if (dto.Observacoes != null) lancamento.Observacoes = dto.Observacoes;

        _repo.Update(lancamento);
        await _uow.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}
