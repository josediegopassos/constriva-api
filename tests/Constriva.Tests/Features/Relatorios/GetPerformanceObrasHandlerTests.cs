using FluentAssertions;
using Moq;
using Constriva.Application.Features.Relatorios;
using Constriva.Domain.Entities.Obras;
using Constriva.Domain.Enums;
using Constriva.Domain.Interfaces.Repositories;
using Xunit;

namespace Constriva.Tests.Features.Relatorios;

public class GetPerformanceObrasHandlerTests
{
    private readonly Mock<IRelatoriosRepository> _repo = new();
    private readonly GetPerformanceObrasHandler _handler;
    private static readonly Guid EmpresaId = Guid.NewGuid();

    public GetPerformanceObrasHandlerTests()
    {
        _handler = new GetPerformanceObrasHandler(_repo.Object);
    }

    [Fact]
    public async Task Handle_DeveRetornarPerformance_SomentePorObrasAtivas()
    {
        _repo.Setup(r => r.GetObrasParaRelatorioAsync(EmpresaId, null, default))
            .ReturnsAsync(new List<Obra>
            {
                new() { Codigo = "OBR-001", Nome = "Obra Ativa",
                    Status = StatusObraEnum.EmAndamento, ValorContrato = 1_000_000m,
                    PercentualConcluido = 50m,
                    DataInicioPrevista = DateTime.Today.AddMonths(-6),
                    DataFimPrevista = DateTime.Today.AddMonths(6) },
                new() { Codigo = "OBR-002", Nome = "Obra Planejamento",
                    Status = StatusObraEnum.Orcamento, ValorContrato = 500_000m,
                    DataInicioPrevista = DateTime.Today.AddMonths(1),
                    DataFimPrevista = DateTime.Today.AddMonths(12) }
            });

        var result = await _handler.Handle(new GetPerformanceObrasQuery(EmpresaId), default);
        var list = result.ToList();

        list.Should().HaveCount(1);
        list[0].ObraNome.Should().Be("Obra Ativa");
    }

    [Fact]
    public async Task Handle_DeveIdentificarObraAtrasada()
    {
        _repo.Setup(r => r.GetObrasParaRelatorioAsync(EmpresaId, null, default))
            .ReturnsAsync(new List<Obra>
            {
                new() { Codigo = "OBR-001", Nome = "Atrasada",
                    Status = StatusObraEnum.EmAndamento, ValorContrato = 500_000m,
                    PercentualConcluido = 30m,
                    DataInicioPrevista = DateTime.Today.AddMonths(-12),
                    DataFimPrevista = DateTime.Today.AddDays(-30) }
            });

        var result = await _handler.Handle(new GetPerformanceObrasQuery(EmpresaId), default);

        result.First().Atrasada.Should().BeTrue();
    }
}
