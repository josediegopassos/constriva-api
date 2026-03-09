using FluentAssertions;
using Moq;
using Constriva.Application.Features.Orcamento.Queries;
using Constriva.Domain.Enums;
using Constriva.Domain.Interfaces.Repositories;
using Xunit;

namespace Constriva.Tests.Features.Orcamento;

public class GetOrcamentoDashboardHandlerTests
{
    private readonly Mock<IOrcamentoRepository> _orcRepo = new();
    private readonly GetOrcamentoDashboardHandler _handler;

    private static readonly Guid EmpresaId = Guid.NewGuid();
    private static readonly Guid ObraId = Guid.NewGuid();

    public GetOrcamentoDashboardHandlerTests()
    {
        _handler = new GetOrcamentoDashboardHandler(_orcRepo.Object);
    }

    [Fact]
    public async Task Handle_DeveRetornarDashboardCorreto()
    {
        var orcamentos = new List<Domain.Entities.Orcamento.Orcamento>
        {
            new() { ObraId = ObraId, EmpresaId = EmpresaId, Status = StatusOrcamentoEnum.Aprovado,
                    ValorTotal = 500_000m, Nome = "V1" },
            new() { ObraId = ObraId, EmpresaId = EmpresaId, Status = StatusOrcamentoEnum.Aprovado,
                    ValorTotal = 600_000m, Nome = "V2" },
            new() { ObraId = ObraId, EmpresaId = EmpresaId, Status = StatusOrcamentoEnum.Rascunho,
                    ValorTotal = 0m, Nome = "V3" },
            new() { ObraId = ObraId, EmpresaId = EmpresaId, Status = StatusOrcamentoEnum.EmAnalise,
                    ValorTotal = 550_000m, Nome = "V4" },
        };

        _orcRepo.Setup(r => r.GetByObraAsync(ObraId, EmpresaId, default)).ReturnsAsync(orcamentos);

        var result = await _handler.Handle(new GetOrcamentoDashboardQuery(ObraId, EmpresaId), default);

        result.TotalOrcamentos.Should().Be(4);
        result.Aprovados.Should().Be(2);
        result.Rascunhos.Should().Be(1);
        result.EmRevisao.Should().Be(1);
        result.ValorTotalAprovados.Should().Be(1_100_000m);
    }
}
