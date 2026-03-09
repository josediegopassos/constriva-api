using Constriva.Application.Features.RH.Commands;
using Constriva.Application.Features.RH.DTOs;
using Constriva.Domain.Entities.RH;
using Constriva.Domain.Enums;
using Constriva.Domain.Interfaces.Repositories;
using FluentAssertions;
using Moq;
using Xunit;

namespace Constriva.Tests.Features.RH;

public class CreateFuncionarioHandlerTests
{
    private readonly Mock<IRHRepository> _repo = new();
    private readonly Mock<IUnitOfWork> _uow = new();
    private readonly CreateFuncionarioCommandHandler _handler;
    private static readonly Guid EmpresaId = Guid.NewGuid();

    public CreateFuncionarioHandlerTests()
    {
        _handler = new CreateFuncionarioCommandHandler(_repo.Object, _uow.Object);
    }

    [Fact]
    public async Task Handle_DeveCriarFuncionario_ComDadosValidos()
    {
        Funcionario? criado = null;
        _repo.Setup(r => r.AddFuncionarioAsync(It.IsAny<Funcionario>(), default))
            .Callback<Funcionario, CancellationToken>((f, _) => criado = f)
            .Returns(Task.CompletedTask);
        _uow.Setup(u => u.SaveChangesAsync(default)).ReturnsAsync(1);

        var dto = new CreateFuncionarioDto("Ana Lima", "987.654.321-00", "ana@email.com",
            null, Guid.NewGuid(), Guid.NewGuid(), DateTime.Today.AddYears(-2), 3500m, StatusFuncionarioEnum.Ativo);

        var result = await _handler.Handle(new CreateFuncionarioCommand(EmpresaId, dto), default);

        result.Should().NotBeNull();
        result.Nome.Should().Be("Ana Lima");
        criado!.EmpresaId.Should().Be(EmpresaId);
    }
}
