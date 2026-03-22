using MediatR;
using Constriva.Application.Common.Behaviors;
using Constriva.Application.Common.Interfaces;
using Constriva.Domain.Entities.Estoque;
using Constriva.Domain.Enums;
using Constriva.Domain.Interfaces.Repositories;
using Constriva.Application.Features.Estoque.DTOs;

namespace Constriva.Application.Features.Estoque.Commands;

public record CreateMovimentacaoDto(
    Guid MaterialId,
    Guid AlmoxarifadoId,
    TipoMovimentacaoEstoqueEnum Tipo,
    decimal Quantidade,
    decimal PrecoUnitario,
    DateTime? DataMovimentacao,
    string? NumeroDocumento,
    string? NumeroNF,
    Guid? ObraId,
    Guid? FaseObraId,
    Guid? AlmoxarifadoDestinoId,
    string? Lote,
    DateTime? Validade,
    string? Observacao);

public record CreateMovimentacaoCommand(Guid EmpresaId, Guid UsuarioId, CreateMovimentacaoDto Dto)
    : IRequest<MovimentacaoEstoqueDto>, ITenantRequest { public Guid TenantId => EmpresaId; }

public class CreateMovimentacaoHandler : IRequestHandler<CreateMovimentacaoCommand, MovimentacaoEstoqueDto>
{
    private readonly IEstoqueRepository _repo;
    private readonly IMaterialRepository _materialRepo;
    private readonly IUnitOfWork _uow;

    public CreateMovimentacaoHandler(IEstoqueRepository repo, IMaterialRepository materialRepo, IUnitOfWork uow)
    {
        _repo = repo;
        _materialRepo = materialRepo;
        _uow = uow;
    }

    public async Task<MovimentacaoEstoqueDto> Handle(CreateMovimentacaoCommand request, CancellationToken cancellationToken)
    {
        var dto = request.Dto;

        var material = await _materialRepo.GetByIdAndEmpresaAsync(dto.MaterialId, request.EmpresaId, cancellationToken)
            ?? throw new KeyNotFoundException($"Material {dto.MaterialId} não encontrado.");

        _ = await _repo.GetAlmoxarifadoByIdAsync(dto.AlmoxarifadoId, request.EmpresaId, cancellationToken)
            ?? throw new KeyNotFoundException($"Almoxarifado {dto.AlmoxarifadoId} não encontrado.");

        if (dto.Tipo == TipoMovimentacaoEstoqueEnum.Transferencia && dto.AlmoxarifadoDestinoId == null)
            throw new InvalidOperationException("Transferência requer AlmoxarifadoDestinoId.");

        if (dto.Quantidade <= 0)
            throw new InvalidOperationException("Quantidade deve ser maior que zero.");

        // Saldo origem
        var saldo = await _repo.GetSaldoAsync(dto.AlmoxarifadoId, dto.MaterialId, request.EmpresaId, cancellationToken);
        if (saldo == null)
        {
            saldo = new EstoqueSaldo
            {
                EmpresaId = request.EmpresaId,
                AlmoxarifadoId = dto.AlmoxarifadoId,
                MaterialId = dto.MaterialId
            };
            await _repo.AddSaldoAsync(saldo, cancellationToken);
        }

        if (dto.Tipo is TipoMovimentacaoEstoqueEnum.Saida or TipoMovimentacaoEstoqueEnum.Transferencia)
        {
            if (saldo.SaldoDisponivel < dto.Quantidade)
                throw new InvalidOperationException(
                    $"Saldo insuficiente. Disponível: {saldo.SaldoDisponivel}, Solicitado: {dto.Quantidade}.");
        }

        var saldoAnterior = saldo.SaldoAtual;

        switch (dto.Tipo)
        {
            case TipoMovimentacaoEstoqueEnum.Entrada:
            case TipoMovimentacaoEstoqueEnum.Devolucao:
                // Atualiza custo médio ponderado na entrada
                if (dto.Tipo == TipoMovimentacaoEstoqueEnum.Entrada && dto.PrecoUnitario > 0)
                {
                    var valorAtual = saldo.SaldoAtual * saldo.CustoMedio;
                    var valorNovo = dto.Quantidade * dto.PrecoUnitario;
                    var novoSaldo = saldo.SaldoAtual + dto.Quantidade;
                    saldo.CustoMedio = novoSaldo > 0 ? (valorAtual + valorNovo) / novoSaldo : dto.PrecoUnitario;
                    material.PrecoUltimaCompra = dto.PrecoUnitario;
                }
                saldo.SaldoAtual += dto.Quantidade;
                break;

            case TipoMovimentacaoEstoqueEnum.Saida:
                saldo.SaldoAtual -= dto.Quantidade;
                break;

            case TipoMovimentacaoEstoqueEnum.Ajuste:
                saldo.SaldoAtual = dto.Quantidade; // Ajuste define o saldo absoluto
                break;

            case TipoMovimentacaoEstoqueEnum.Transferencia:
                saldo.SaldoAtual -= dto.Quantidade;
                var saldoDestino = await _repo.GetSaldoAsync(dto.AlmoxarifadoDestinoId!.Value, dto.MaterialId, request.EmpresaId, cancellationToken);
                if (saldoDestino == null)
                {
                    saldoDestino = new EstoqueSaldo
                    {
                        EmpresaId = request.EmpresaId,
                        AlmoxarifadoId = dto.AlmoxarifadoDestinoId.Value,
                        MaterialId = dto.MaterialId,
                        CustoMedio = saldo.CustoMedio
                    };
                    await _repo.AddSaldoAsync(saldoDestino, cancellationToken);
                }
                saldoDestino.SaldoAtual += dto.Quantidade;
                saldoDestino.UltimaMovimentacao = DateTime.UtcNow;
                break;
        }

        saldo.UltimaMovimentacao = DateTime.UtcNow;

        var movimentacao = new MovimentacaoEstoque
        {
            EmpresaId = request.EmpresaId,
            AlmoxarifadoId = dto.AlmoxarifadoId,
            MaterialId = dto.MaterialId,
            ObraId = dto.ObraId,
            FaseObraId = dto.FaseObraId,
            AlmoxarifadoDestinoId = dto.AlmoxarifadoDestinoId,
            Tipo = dto.Tipo,
            Quantidade = dto.Quantidade,
            PrecoUnitario = dto.PrecoUnitario,
            SaldoAnterior = saldoAnterior,
            SaldoPosterior = saldo.SaldoAtual,
            DataMovimentacao = dto.DataMovimentacao ?? DateTime.UtcNow,
            NumeroDocumento = dto.NumeroDocumento,
            NumeroNF = dto.NumeroNF,
            Lote = dto.Lote,
            Validade = dto.Validade,
            Observacao = dto.Observacao,
            UsuarioId = request.UsuarioId
        };

        await _repo.AddMovimentacaoAsync(movimentacao, cancellationToken);
        await _uow.SaveChangesAsync(cancellationToken);

        return new MovimentacaoEstoqueDto(
            movimentacao.Id, movimentacao.Tipo, movimentacao.Tipo.ToString(),
            movimentacao.AlmoxarifadoId, "",
            movimentacao.MaterialId, material.Nome, material.Codigo,
            movimentacao.Quantidade, movimentacao.PrecoUnitario, movimentacao.ValorTotal,
            movimentacao.SaldoAnterior, movimentacao.SaldoPosterior,
            movimentacao.DataMovimentacao, movimentacao.NumeroDocumento, movimentacao.NumeroNF, movimentacao.Lote,
            movimentacao.ObraId, movimentacao.UsuarioId, "", movimentacao.CreatedAt);
    }
}
