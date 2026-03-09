using Constriva.Application.Features.Cronograma.Commands;
using Constriva.Domain.Entities.Cronograma;
using Constriva.Domain.Enums;
using Constriva.Domain.Interfaces.Repositories;
using FluentAssertions;
using Moq;
using Xunit;

namespace Constriva.Tests.Features.Cronograma;

public class UpdateProgressoAtividadeHandlerTests
{
    private readonly Mock<ICronogramaRepository> _cronogramaRepo = new();
    private readonly Mock<IUnitOfWork> _uow = new();
    private readonly UpdateProgressoAtividadeCommandHandler _handler;

    private static readonly Guid EmpresaId = Guid.NewGuid();

    public UpdateProgressoAtividadeHandlerTests()
    {
        _handler = new UpdateProgressoAtividadeCommandHandler(_cronogramaRepo.Object, _uow.Object);
    }

    [Fact]
    public async Task Handle_DeveAtualizarProgressoEStatus_QuandoIniciada()
    {
        var atividadeId = Guid.NewGuid();
        var atividade = new AtividadeCronograma
        {
            EmpresaId = EmpresaId,
            PercentualConcluido = 0, Status = StatusAtividadeEnum.NaoIniciada
        };
        _cronogramaRepo.Setup(r => r.GetAtividadeByIdAsync(It.IsAny<Guid>(), EmpresaId, default))
            .ReturnsAsync(atividade);
        _uow.Setup(u => u.SaveChangesAsync(default)).ReturnsAsync(1);

        await _handler.Handle(
            new UpdateProgressoAtividadeCommand(atividadeId, EmpresaId, 50), default);

        atividade.PercentualConcluido.Should().Be(50);
        atividade.Status.Should().Be(StatusAtividadeEnum.EmAndamento);
    }

    [Fact]
    public async Task Handle_DeveMarcarConcluida_QuandoProgressoE100()
    {
        var atividadeId = Guid.NewGuid();
        var atividade = new AtividadeCronograma
        {
            EmpresaId = EmpresaId,
            PercentualConcluido = 80, Status = StatusAtividadeEnum.EmAndamento
        };
        _cronogramaRepo.Setup(r => r.GetAtividadeByIdAsync(It.IsAny<Guid>(), EmpresaId, default))
            .ReturnsAsync(atividade);
        _uow.Setup(u => u.SaveChangesAsync(default)).ReturnsAsync(1);

        await _handler.Handle(
            new UpdateProgressoAtividadeCommand(atividadeId, EmpresaId, 100), default);

        atividade.PercentualConcluido.Should().Be(100);
        atividade.Status.Should().Be(StatusAtividadeEnum.Concluida);
    }

    [Fact]
    public async Task Handle_DeveLancarExcecao_QuandoProgressoMaiorQue100()
    {
        var atividadeId = Guid.NewGuid();
        var atividade = new AtividadeCronograma { EmpresaId = EmpresaId };
        _cronogramaRepo.Setup(r => r.GetAtividadeByIdAsync(It.IsAny<Guid>(), EmpresaId, default))
            .ReturnsAsync(atividade);

        await Assert.ThrowsAsync<InvalidOperationException>(
            () => _handler.Handle(
                new UpdateProgressoAtividadeCommand(atividadeId, EmpresaId, 105), default));
    }
}
