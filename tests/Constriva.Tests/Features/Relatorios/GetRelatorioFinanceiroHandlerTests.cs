using FluentAssertions;
using Moq;
using Constriva.Application.Features.Relatorios;
using Constriva.Domain.Entities.Financeiro;
using Constriva.Domain.Enums;
using Constriva.Domain.Interfaces.Repositories;
using Xunit;

namespace Constriva.Tests.Features.Relatorios;

public class GetRelatorioFinanceiroHandlerTests
{
    private readonly Mock<ILancamentoFinanceiroRepository> _repo = new();
    private readonly GetRelatorioFinanceiroHandler _handler;
    private static readonly Guid EmpresaId = Guid.NewGuid();

    public GetRelatorioFinanceiroHandlerTests()
    {
        _handler = new GetRelatorioFinanceiroHandler(_repo.Object);
    }

    [Fact]
    public async Task Handle_DeveCalcularSaldo_CorretamentePorReceeitasEDespesas()
    {
        var lancamentos = new List<LancamentoFinanceiro>
        {
            new() { Tipo = TipoLancamentoEnum.Receita, Valor = 100_000m,
                Status = StatusLancamentoEnum.Realizado, ValorRealizado = 100_000m,
                DataVencimento = DateTime.Today },
            new() { Tipo = TipoLancamentoEnum.Despesa, Valor = 60_000m,
                Status = StatusLancamentoEnum.Realizado, ValorRealizado = 60_000m,
                DataVencimento = DateTime.Today }
        };
        _repo.Setup(r => r.GetAllByEmpresaAsync(EmpresaId, default)).ReturnsAsync(lancamentos);

        var result = await _handler.Handle(new GetRelatorioFinanceiroQuery(EmpresaId), default);

        result.TotalReceitas.Should().Be(100_000m);
        result.TotalDespesas.Should().Be(60_000m);
        result.Saldo.Should().Be(40_000m);
        result.ReceitasPagas.Should().Be(100_000m);
        result.DespesasPagas.Should().Be(60_000m);
    }

    [Fact]
    public async Task Handle_DeveContabilizarDespesasVencidas()
    {
        var lancamentos = new List<LancamentoFinanceiro>
        {
            new() { Tipo = TipoLancamentoEnum.Despesa, Valor = 10_000m,
                Status = StatusLancamentoEnum.Previsto,
                DataVencimento = DateTime.Today.AddDays(-10) }
        };
        _repo.Setup(r => r.GetAllByEmpresaAsync(EmpresaId, default)).ReturnsAsync(lancamentos);

        var result = await _handler.Handle(new GetRelatorioFinanceiroQuery(EmpresaId), default);

        result.DespesasVencidas.Should().Be(10_000m);
    }

    [Fact]
    public async Task Handle_DeveGerarFluxoMensalAgrupado()
    {
        var lancamentos = new List<LancamentoFinanceiro>
        {
            new() { Tipo = TipoLancamentoEnum.Receita, Valor = 50_000m,
                Status = StatusLancamentoEnum.Previsto, DataVencimento = new DateTime(2025, 1, 15) },
            new() { Tipo = TipoLancamentoEnum.Despesa, Valor = 30_000m,
                Status = StatusLancamentoEnum.Previsto, DataVencimento = new DateTime(2025, 1, 20) },
            new() { Tipo = TipoLancamentoEnum.Receita, Valor = 40_000m,
                Status = StatusLancamentoEnum.Previsto, DataVencimento = new DateTime(2025, 2, 10) }
        };
        _repo.Setup(r => r.GetAllByEmpresaAsync(EmpresaId, default)).ReturnsAsync(lancamentos);

        var result = await _handler.Handle(new GetRelatorioFinanceiroQuery(EmpresaId), default);

        result.FluxoMensal.Should().HaveCount(2);
        var jan = result.FluxoMensal.FirstOrDefault(f => f.Mes == 1 && f.Ano == 2025);
        jan.Should().NotBeNull();
        jan!.Saldo.Should().Be(20_000m);
    }
}
