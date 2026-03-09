using Constriva.Application.Features.Obras.Commands;
using Constriva.Domain.Entities.Obras;
using Constriva.Domain.Interfaces.Repositories;
using FluentAssertions;
using Moq;
using Xunit;

namespace Constriva.Tests.Features.Obras;

public class UpdatePercentualHandlerTests
{
    private readonly Mock<IObraRepository> _obraRepo = new();
    private readonly Mock<IUnitOfWork> _uow = new();
    private readonly UpdatePercentualObraCommandHandler _handler;

    private static readonly Guid EmpresaId = Guid.NewGuid();

    public UpdatePercentualHandlerTests()
    {
        _uow.Setup(u => u.SaveChangesAsync(default)).ReturnsAsync(1);
        _handler = new UpdatePercentualObraCommandHandler(_obraRepo.Object, _uow.Object);
    }

    [Fact]
    public async Task Handle_DeveDefinirDataFimReal_QuandoPercentualAtinge100()
    {
        var obraId = Guid.NewGuid();
        var obra = new Obra { EmpresaId = EmpresaId, PercentualConcluido = 80, DataFimReal = null };
        _obraRepo.Setup(r => r.GetByIdAndEmpresaAsync(obraId, EmpresaId, default)).ReturnsAsync(obra);

        await _handler.Handle(new UpdatePercentualObraCommand(obraId, EmpresaId, 100), default);

        obra.PercentualConcluido.Should().Be(100);
        obra.DataFimReal.Should().Be(DateTime.Today);
    }

    [Fact]
    public async Task Handle_NaoDeveSubstituirDataFimReal_SeJaDefinida()
    {
        var obraId = Guid.NewGuid();
        var dataExistente = DateTime.Today.AddDays(-5);
        var obra = new Obra { EmpresaId = EmpresaId, PercentualConcluido = 80, DataFimReal = dataExistente };
        _obraRepo.Setup(r => r.GetByIdAndEmpresaAsync(obraId, EmpresaId, default)).ReturnsAsync(obra);

        await _handler.Handle(new UpdatePercentualObraCommand(obraId, EmpresaId, 100), default);

        obra.DataFimReal.Should().Be(dataExistente);
    }

    [Fact]
    public async Task Handle_NaoDeveDefinirDataFimReal_QuandoPercentualMenorQue100()
    {
        var obraId = Guid.NewGuid();
        var obra = new Obra { EmpresaId = EmpresaId, PercentualConcluido = 50, DataFimReal = null };
        _obraRepo.Setup(r => r.GetByIdAndEmpresaAsync(obraId, EmpresaId, default)).ReturnsAsync(obra);

        await _handler.Handle(new UpdatePercentualObraCommand(obraId, EmpresaId, 75), default);

        obra.DataFimReal.Should().BeNull();
    }
}
