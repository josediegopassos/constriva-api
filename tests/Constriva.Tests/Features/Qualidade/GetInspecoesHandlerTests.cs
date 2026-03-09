using Constriva.Application.Features.Qualidade;
using Constriva.Domain.Entities.Qualidade;
using Constriva.Domain.Enums;
using Constriva.Domain.Interfaces.Repositories;
using FluentAssertions;
using Moq;
using Xunit;

namespace Constriva.Tests.Features.Qualidade;

public class GetInspecoesHandlerTests
{
    private readonly Mock<IQualidadeRepository> _repo = new();
    private readonly GetInspecoesHandler _handler;
    private static readonly Guid EmpresaId = Guid.NewGuid();

    public GetInspecoesHandlerTests()
    {
        _handler = new GetInspecoesHandler(_repo.Object);
    }

    [Fact]
    public async Task Handle_DeveRetornarInspecoesPaginadas()
    {
        var inspecoes = new List<Inspecao>
        {
            new() { Numero = "INS-001", Titulo = "Inspeção Pré-Serviço",
                Status = StatusInspecaoEnum.Pendente, DataProgramada = DateTime.Today.AddDays(3) }
        };
        _repo.Setup(r => r.GetInspecoesPagedAsync(EmpresaId, null, null, 1, 20, default))
            .ReturnsAsync((inspecoes, 1));

        var result = await _handler.Handle(new GetInspecoesQuery(EmpresaId), default);

        result.TotalCount.Should().Be(1);
        result.Items.First().Numero.Should().Be("INS-001");
    }

    [Fact]
    public async Task Handle_DeveFiltrarPorStatus()
    {
        _repo.Setup(r => r.GetInspecoesPagedAsync(EmpresaId, null, StatusInspecaoEnum.Aprovada, 1, 20, default))
            .ReturnsAsync((new List<Inspecao>(), 0));

        var result = await _handler.Handle(
            new GetInspecoesQuery(EmpresaId, null, StatusInspecaoEnum.Aprovada), default);

        _repo.Verify(r => r.GetInspecoesPagedAsync(EmpresaId, null, StatusInspecaoEnum.Aprovada, 1, 20, default), Times.Once);
    }
}
