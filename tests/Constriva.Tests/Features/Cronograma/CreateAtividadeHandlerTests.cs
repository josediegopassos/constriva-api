using Constriva.Application.Features.Cronograma.Commands;
using Constriva.Application.Features.Cronograma.DTOs;
using Constriva.Domain.Entities.Cronograma;
using Constriva.Domain.Enums;
using Constriva.Domain.Interfaces.Repositories;
using FluentAssertions;
using Moq;
using Xunit;

namespace Constriva.Tests.Features.Cronograma;

public class CreateAtividadeHandlerTests
{
    private readonly Mock<ICronogramaRepository> _cronogramaRepo = new();
    private readonly Mock<IUnitOfWork> _uow = new();
    private readonly CreateAtividadeCommandHandler _handler;

    private static readonly Guid EmpresaId = Guid.NewGuid();
    private static readonly Guid ObraId = Guid.NewGuid();

    public CreateAtividadeHandlerTests()
    {
        _handler = new CreateAtividadeCommandHandler(_cronogramaRepo.Object, _uow.Object);
    }

    [Fact]
    public async Task Handle_DeveCriarAtividade_ComDadosValidos()
    {
        var cronograma = new CronogramaObra { EmpresaId = EmpresaId, ObraId = ObraId };
        _cronogramaRepo.Setup(r => r.GetByObraAsync(ObraId, EmpresaId, default)).ReturnsAsync(cronograma);
        _cronogramaRepo.Setup(r => r.GetMaxOrdemAsync(It.IsAny<Guid>(), default)).ReturnsAsync(0);
        _cronogramaRepo.Setup(r => r.AddAtividadeAsync(It.IsAny<AtividadeCronograma>(), default))
            .Returns(Task.CompletedTask);
        _uow.Setup(u => u.SaveChangesAsync(default)).ReturnsAsync(1);

        var dto = new CreateAtividadeDto(
            "Fundação", "Execução da fundação do edifício",
            1, DateTime.Today, DateTime.Today.AddDays(30), 30m);

        var result = await _handler.Handle(
            new CreateAtividadeCommand(ObraId, EmpresaId, dto), default);

        result.Should().NotBeNull();
        result.Nome.Should().Be("Fundação");
        result.PercentualConcluido.Should().Be(0);
        result.Status.Should().Be(StatusAtividadeEnum.NaoIniciada);
    }

    [Fact]
    public async Task Handle_DeveDefinirOrdemSequencial_QuandoJaExistemAtividades()
    {
        var cronograma = new CronogramaObra { EmpresaId = EmpresaId, ObraId = ObraId };
        _cronogramaRepo.Setup(r => r.GetByObraAsync(ObraId, EmpresaId, default)).ReturnsAsync(cronograma);
        _cronogramaRepo.Setup(r => r.GetMaxOrdemAsync(It.IsAny<Guid>(), default)).ReturnsAsync(5);
        AtividadeCronograma? atividadeCriada = null;
        _cronogramaRepo.Setup(r => r.AddAtividadeAsync(It.IsAny<AtividadeCronograma>(), default))
            .Callback<AtividadeCronograma, CancellationToken>((a, _) => atividadeCriada = a)
            .Returns(Task.CompletedTask);
        _uow.Setup(u => u.SaveChangesAsync(default)).ReturnsAsync(1);

        var dto = new CreateAtividadeDto("Alvenaria", null, 6, DateTime.Today,
            DateTime.Today.AddDays(20), 20m);

        await _handler.Handle(new CreateAtividadeCommand(ObraId, EmpresaId, dto), default);

        atividadeCriada!.Ordem.Should().Be(6);
    }
}
