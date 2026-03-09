using Constriva.Application.Features.SST.Commands;
using Constriva.Application.Features.SST.DTOs;
using Constriva.Domain.Entities.SST;
using Constriva.Domain.Interfaces.Repositories;
using FluentAssertions;
using Moq;
using Xunit;

namespace Constriva.Tests.Features.SST;

public class CreateDDSHandlerTests
{
    private readonly Mock<ISSTRepository> _repo = new();
    private readonly Mock<IUnitOfWork> _uow = new();
    private readonly CreateDDSCommandHandler _handler;

    public CreateDDSHandlerTests()
    {
        _handler = new CreateDDSCommandHandler(_repo.Object, _uow.Object);
    }

    [Fact]
    public async Task Handle_DeveCriarDDS_ERetornarDto()
    {
        _repo.Setup(r => r.AddDDSAsync(It.IsAny<DDS>(), default)).Returns(Task.CompletedTask);
        _uow.Setup(u => u.SaveChangesAsync(default)).ReturnsAsync(1);

        var empresaId = Guid.NewGuid();
        var obraId = Guid.NewGuid();
        var cmd = new CreateDDSCommand(empresaId, Guid.NewGuid(),
            new CreateDDSDto(obraId, "DDS-001", "EPI obrigatório", "Conteúdo", "Eng. Carlos", 20, DateTime.Today));

        var result = await _handler.Handle(cmd, default);

        result.Should().NotBeNull();
        result.Tema.Should().Be("EPI obrigatório");
        result.NumeroParticipantes.Should().Be(20);
        _repo.Verify(r => r.AddDDSAsync(It.IsAny<DDS>(), default), Times.Once);
        _uow.Verify(u => u.SaveChangesAsync(default), Times.Once);
    }
}
