using MediatR;
using Constriva.Application.Common.Behaviors;
using Constriva.Application.Features.Financeiro;
using Constriva.Domain.Entities.Financeiro;
using Constriva.Domain.Enums;
using Constriva.Domain.Interfaces.Repositories;
using Constriva.Application.Features.Financeiro.DTOs;

namespace Constriva.Application.Features.Financeiro.Commands;

public record CreateLancamentoCommand(Guid EmpresaId, Guid UsuarioId, CreateLancamentoDto Dto)
    : IRequest<LancamentoFinanceiroDto>, ITenantRequest { public Guid TenantId => EmpresaId; }

public class CreateLancamentoHandler : IRequestHandler<CreateLancamentoCommand, LancamentoFinanceiroDto>
{
    private readonly ILancamentoFinanceiroRepository _repo;
    private readonly IObraRepository _obraRepo;
    private readonly IFornecedorRepository _fornecedorRepo;
    private readonly ICentroCustoRepository _centroCustoRepo;
    private readonly IContaBancariaRepository _contaBancariaRepo;
    private readonly IUnitOfWork _uow;

    public CreateLancamentoHandler(
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

    public async Task<LancamentoFinanceiroDto> Handle(CreateLancamentoCommand request, CancellationToken cancellationToken)
    {
        var dto = request.Dto;

        // Valida FKs opcionais quando informadas
        if (dto.ObraId.HasValue)
            _ = await _obraRepo.GetByIdAndEmpresaAsync(dto.ObraId.Value, request.EmpresaId, cancellationToken)
                ?? throw new KeyNotFoundException($"Obra {dto.ObraId} não encontrada.");

        if (dto.FornecedorId.HasValue)
            _ = await _fornecedorRepo.GetByIdAndEmpresaAsync(dto.FornecedorId.Value, request.EmpresaId, cancellationToken)
                ?? throw new KeyNotFoundException($"Fornecedor {dto.FornecedorId} não encontrado.");

        if (dto.CentroCustoId.HasValue)
            _ = await _centroCustoRepo.GetByIdAndEmpresaAsync(dto.CentroCustoId.Value, request.EmpresaId, cancellationToken)
                ?? throw new KeyNotFoundException($"Centro de custo {dto.CentroCustoId} não encontrado.");

        if (dto.ContaBancariaId.HasValue)
            _ = await _contaBancariaRepo.GetByIdAndEmpresaAsync(dto.ContaBancariaId.Value, request.EmpresaId, cancellationToken)
                ?? throw new KeyNotFoundException($"Conta bancária {dto.ContaBancariaId} não encontrada.");

        var lancamento = new LancamentoFinanceiro
        {
            EmpresaId = request.EmpresaId,
            CriadoPor = request.UsuarioId,
            Tipo = dto.Tipo,
            Descricao = dto.Descricao,
            Valor = dto.Valor,
            DataVencimento = dto.DataVencimento,
            ObraId = dto.ObraId,
            FornecedorId = dto.FornecedorId,
            CentroCustoId = dto.CentroCustoId,
            ContaBancariaId = dto.ContaBancariaId,
            FormaPagamentoEnum = dto.FormaPagamentoEnum,
            NumeroDocumento = dto.NumeroDocumento,
            NumeroNF = dto.NumeroNF,
            Observacoes = dto.Observacoes,
            Repetir = dto.Repetir,
            Periodicidade = dto.Periodicidade,
            QtdParcelas = dto.QtdParcelas,
            Status = StatusLancamentoEnum.Previsto
        };

        await _repo.AddAsync(lancamento, cancellationToken);
        await _uow.SaveChangesAsync(cancellationToken);

        return new LancamentoFinanceiroDto(
            lancamento.Id, lancamento.Descricao, lancamento.Tipo, lancamento.Status,
            lancamento.Valor, lancamento.ValorRealizado, lancamento.DataVencimento,
            lancamento.DataPagamento, lancamento.FormaPagamentoEnum,
            lancamento.NumeroDocumento, lancamento.NumeroNF, lancamento.Observacoes,
            lancamento.ObraId, null, lancamento.FornecedorId, null,
            lancamento.CentroCustoId, null, lancamento.CreatedAt);
    }
}
