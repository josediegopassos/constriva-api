using Constriva.Application.Features.Qualidade.Commands;
using Constriva.Application.Features.Qualidade.DTOs;
using Constriva.Domain.Entities.Qualidade;
using Constriva.Domain.Enums;
using Constriva.Domain.Interfaces.Repositories;
using FluentAssertions;
using Moq;
using Xunit;

namespace Constriva.Tests.Features.Qualidade;

public class CreateInspecaoHandlerTests
{
    private readonly Mock<IQualidadeRepository> _repo = new();
    private readonly Mock<IUnitOfWork> _uow = new();
    private readonly CreateInspecaoCommandHandler _handler;
    private static readonly Guid EmpresaId = Guid.NewGuid();

    public CreateInspecaoHandlerTests()
    {
        _handler = new CreateInspecaoCommandHandler(_repo.Object, _uow.Object);
    }

    [Fact]
    public async Task Handle_DeveCriarInspecao_ComStatusPendente()
    {
        Inspecao? criada = null;
        _repo.Setup(r => r.AddInspecaoAsync(It.IsAny<Inspecao>(), default))
            .Callback<Inspecao, CancellationToken>((i, _) => criada = i)
            .Returns(Task.CompletedTask);
        _uow.Setup(u => u.SaveChangesAsync(default)).ReturnsAsync(1);

        var dto = new CreateInspecaoDto(Guid.NewGuid(), "INS-002", "Inspeção Rotina", null,
            DateTime.Today.AddDays(5), null, null);
        await _handler.Handle(new CreateInspecaoCommand(EmpresaId, dto), default);

        criada.Should().NotBeNull();
        criada!.Status.Should().Be(StatusInspecaoEnum.Pendente);
        criada.EmpresaId.Should().Be(EmpresaId);
    }
}
