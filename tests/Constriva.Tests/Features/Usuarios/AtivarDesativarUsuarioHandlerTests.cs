using Constriva.Application.Features.Usuarios.Commands;
using Constriva.Domain.Entities.Tenant;
using Constriva.Domain.Interfaces.Repositories;
using FluentAssertions;
using Moq;
using Xunit;

namespace Constriva.Tests.Features.Usuarios;

public class AtivarDesativarUsuarioHandlerTests
{
    private readonly Mock<IUsuarioRepository> _usuarioRepo = new();
    private readonly Mock<IUnitOfWork> _uow = new();
    private readonly AtivarDesativarUsuarioHandler _handler;
    private static readonly Guid EmpresaId = Guid.NewGuid();

    public AtivarDesativarUsuarioHandlerTests()
    {
        _handler = new AtivarDesativarUsuarioHandler(_usuarioRepo.Object, _uow.Object);
    }

    [Theory]
    [InlineData(true, false)]
    [InlineData(false, true)]
    public async Task Handle_DeveAlternarAtivoCorretamente(bool estadoAtual, bool novoEstado)
    {
        var usuarioId = Guid.NewGuid();
        var usuario = new Usuario { EmpresaId = EmpresaId, Ativo = estadoAtual };
        _usuarioRepo.Setup(r => r.GetByIdAsync(usuarioId, default)).ReturnsAsync(usuario);
        _uow.Setup(u => u.SaveChangesAsync(default)).ReturnsAsync(1);

        await _handler.Handle(new AtivarDesativarUsuarioCommand(usuarioId, EmpresaId, false, novoEstado), default);

        usuario.Ativo.Should().Be(novoEstado);
    }

    [Fact]
    public async Task Handle_NaoDeveSalvar_QuandoUsuarioNaoEncontrado()
    {
        var usuarioId = Guid.NewGuid();
        _usuarioRepo.Setup(r => r.GetByIdAsync(usuarioId, default)).ReturnsAsync((Usuario?)null);

        var result = await _handler.Handle(new AtivarDesativarUsuarioCommand(usuarioId, EmpresaId, false, true), default);

        result.Should().BeFalse();
        _uow.Verify(u => u.SaveChangesAsync(default), Times.Never);
    }
}
