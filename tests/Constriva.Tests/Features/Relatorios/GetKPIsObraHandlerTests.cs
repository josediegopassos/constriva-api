using FluentAssertions;
using Moq;
using Constriva.Application.Features.Relatorios;
using Constriva.Domain.Entities.Financeiro;
using Constriva.Domain.Entities.Obras;
using Constriva.Domain.Entities.Cronograma;
using Constriva.Domain.Interfaces.Repositories;
using Xunit;

namespace Constriva.Tests.Features.Relatorios;

public class GetKPIsObraHandlerTests
{
    private readonly Mock<IObraRepository> _obraRepo = new();
    private readonly Mock<IRelatoriosRepository> _relRepo = new();
    private readonly Mock<ILancamentoFinanceiroRepository> _finRepo = new();
    private readonly GetKPIsObraHandler _handler;

    private static readonly Guid EmpresaId = Guid.NewGuid();
    private static readonly Guid ObraId = Guid.NewGuid();

    public GetKPIsObraHandlerTests()
    {
        _handler = new GetKPIsObraHandler(_obraRepo.Object, _relRepo.Object, _finRepo.Object);
    }

    [Fact]
    public async Task Handle_DeveRetornarNull_QuandoObraInexistente()
    {
        _obraRepo.Setup(r => r.GetByIdAndEmpresaAsync(ObraId, EmpresaId, default))
            .ReturnsAsync((Obra?)null);

        var result = await _handler.Handle(new GetKPIsObraQuery(ObraId, EmpresaId), default);

        result.Should().BeNull();
    }

    [Fact]
    public async Task Handle_DeveCalcularAtividadesAtrasadas()
    {
        var obra = new Obra { EmpresaId = EmpresaId, Nome = "KPI Test",
            ValorContrato = 500_000m, PercentualConcluido = 60m,
            DataInicioPrevista = DateTime.Today.AddMonths(-6),
            DataFimPrevista = DateTime.Today.AddMonths(6) };

        _obraRepo.Setup(r => r.GetByIdAndEmpresaAsync(ObraId, EmpresaId, default)).ReturnsAsync(obra);
        _relRepo.Setup(r => r.GetAtividadesParaKPIAsync(EmpresaId, ObraId, default))
            .ReturnsAsync(new List<AtividadeCronograma>
            {
                new() { PercentualConcluido = 100, DataFimPlanejada = DateTime.Today.AddDays(-5) },
                new() { PercentualConcluido = 30, DataFimPlanejada = DateTime.Today.AddDays(-3) },
                new() { PercentualConcluido = 50, DataFimPlanejada = DateTime.Today.AddDays(10) }
            });
        _finRepo.Setup(r => r.GetByObraAsync(EmpresaId, ObraId, default))
            .ReturnsAsync(new List<LancamentoFinanceiro>());

        var result = await _handler.Handle(new GetKPIsObraQuery(ObraId, EmpresaId), default);

        result.Should().NotBeNull();
        result!.AtividadesTotal.Should().Be(3);
        result.AtividadesConcluidas.Should().Be(1);
        result.AtividadesAtrasadas.Should().Be(1);
    }
}
