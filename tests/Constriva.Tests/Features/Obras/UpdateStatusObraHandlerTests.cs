using Constriva.Application.Features.Obras.Commands;
using Constriva.Domain.Entities.Obras;
using Constriva.Domain.Enums;
using Constriva.Domain.Interfaces.Repositories;
using FluentAssertions;
using Moq;
using Xunit;

namespace Constriva.Tests.Features.Obras;

public class UpdateStatusObraHandlerTests
{
    private readonly Mock<IObraRepository> _obraRepo = new();
    private readonly Mock<IUnitOfWork> _uow = new();
    private readonly UpdateStatusObraCommandHandler _handler;

    private static readonly Guid EmpresaId = Guid.NewGuid();
    private static readonly Guid UserId = Guid.NewGuid();

    public UpdateStatusObraHandlerTests()
    {
        _handler = new UpdateStatusObraCommandHandler(_obraRepo.Object, _uow.Object);
    }

    [Theory]
    [InlineData(StatusObraEnum.Orcamento, StatusObraEnum.Aprovada)]
    [InlineData(StatusObraEnum.Aprovada, StatusObraEnum.EmAndamento)]
    [InlineData(StatusObraEnum.EmAndamento, StatusObraEnum.Concluida)]
    [InlineData(StatusObraEnum.EmAndamento, StatusObraEnum.Paralisada)]
    [InlineData(StatusObraEnum.Paralisada, StatusObraEnum.EmAndamento)]
    public async Task Handle_DeveAtualizarStatus_ParaTransicaoValida(
        StatusObraEnum statusAtual, StatusObraEnum novoStatus)
    {
        var obra = new Obra { EmpresaId = EmpresaId, Status = statusAtual };
        _obraRepo.Setup(r => r.GetByIdAndEmpresaAsync(It.IsAny<Guid>(), EmpresaId, default))
            .ReturnsAsync(obra);
        _obraRepo.Setup(r => r.Update(It.IsAny<Obra>()));
        _obraRepo.Setup(r => r.AddHistoricoAsync(It.IsAny<ObraHistorico>(), default)).Returns(Task.CompletedTask);
        _uow.Setup(u => u.SaveChangesAsync(default)).ReturnsAsync(1);

        await _handler.Handle(
            new UpdateStatusObraCommand(Guid.NewGuid(), EmpresaId, UserId, novoStatus, "Mudança de status"),
            default);

        obra.Status.Should().Be(novoStatus);
    }

    [Theory]
    [InlineData(StatusObraEnum.Orcamento, StatusObraEnum.Concluida)]
    [InlineData(StatusObraEnum.Orcamento, StatusObraEnum.EmAndamento)]
    [InlineData(StatusObraEnum.Concluida, StatusObraEnum.EmAndamento)]
    [InlineData(StatusObraEnum.Cancelada, StatusObraEnum.Aprovada)]
    public async Task Handle_DeveLancar_QuandoTransicaoInvalida(
        StatusObraEnum statusAtual, StatusObraEnum novoStatus)
    {
        var obra = new Obra { EmpresaId = EmpresaId, Status = statusAtual };
        _obraRepo.Setup(r => r.GetByIdAndEmpresaAsync(It.IsAny<Guid>(), EmpresaId, default))
            .ReturnsAsync(obra);

        await Assert.ThrowsAsync<InvalidOperationException>(
            () => _handler.Handle(
                new UpdateStatusObraCommand(Guid.NewGuid(), EmpresaId, UserId, novoStatus, null),
                default));
    }

    [Fact]
    public async Task Handle_DeveDefinirDataInicioReal_QuandoTransicionaParaEmAndamento()
    {
        var obra = new Obra { EmpresaId = EmpresaId, Status = StatusObraEnum.Aprovada, DataInicioReal = null };
        _obraRepo.Setup(r => r.GetByIdAndEmpresaAsync(It.IsAny<Guid>(), EmpresaId, default)).ReturnsAsync(obra);
        _obraRepo.Setup(r => r.AddHistoricoAsync(It.IsAny<ObraHistorico>(), default)).Returns(Task.CompletedTask);
        _uow.Setup(u => u.SaveChangesAsync(default)).ReturnsAsync(1);

        await _handler.Handle(
            new UpdateStatusObraCommand(Guid.NewGuid(), EmpresaId, UserId, StatusObraEnum.EmAndamento, null),
            default);

        obra.DataInicioReal.Should().Be(DateTime.Today);
    }

    [Fact]
    public async Task Handle_DeveRegistrarHistorico_AoAlterarStatus()
    {
        var obra = new Obra { EmpresaId = EmpresaId, Status = StatusObraEnum.Orcamento };
        _obraRepo.Setup(r => r.GetByIdAndEmpresaAsync(It.IsAny<Guid>(), EmpresaId, default)).ReturnsAsync(obra);
        _obraRepo.Setup(r => r.AddHistoricoAsync(It.IsAny<ObraHistorico>(), default)).Returns(Task.CompletedTask);
        _uow.Setup(u => u.SaveChangesAsync(default)).ReturnsAsync(1);

        await _handler.Handle(
            new UpdateStatusObraCommand(Guid.NewGuid(), EmpresaId, UserId, StatusObraEnum.Aprovada, null),
            default);

        _obraRepo.Verify(r => r.AddHistoricoAsync(
            It.Is<ObraHistorico>(h => h.Acao == "StatusAlterado" && h.ValorAnterior == "Orcamento"),
            default), Times.Once);
    }
}
