using Constriva.Application.Features.Usuarios.Commands;
using Constriva.Application.Features.Usuarios.DTOs;
using Constriva.Domain.Entities.Tenant;
using Constriva.Domain.Enums;
using Constriva.Domain.Interfaces.Repositories;
using FluentAssertions;
using Moq;
using Xunit;

namespace Constriva.Tests.Features.Usuarios;

public class UpdateUsuarioHandlerTests
{
    private readonly Mock<IUsuarioRepository> _usuarioRepo = new();
    private readonly Mock<IUnitOfWork> _uow = new();
    private readonly UpdateUsuarioHandler _handler;
    private static readonly Guid EmpresaId = Guid.NewGuid();

    public UpdateUsuarioHandlerTests()
    {
        _uow.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);
        _handler = new UpdateUsuarioHandler(_usuarioRepo.Object, _uow.Object);
    }

    [Fact]
    public async Task Handle_DeveLancar_QuandoNaoSuperAdminAtribuiSuperAdmin()
    {
        var usuarioId = Guid.NewGuid();
        var usuario = new Usuario { EmpresaId = EmpresaId, Perfil = PerfilUsuarioEnum.Colaborador };
        _usuarioRepo.Setup(r => r.GetByIdAsync(usuarioId, default)).ReturnsAsync(usuario);

        var dto = new UpdateUsuarioDto("Nome", null, null, PerfilUsuarioEnum.SuperAdmin);
        await Assert.ThrowsAsync<UnauthorizedAccessException>(
            () => _handler.Handle(new UpdateUsuarioCommand(usuarioId, EmpresaId, false, dto), default));
    }

    [Fact]
    public async Task Handle_DeveLancar_QuandoPromoveParaAdminSemPermissao()
    {
        var usuarioId = Guid.NewGuid();
        var usuario = new Usuario { EmpresaId = EmpresaId, Perfil = PerfilUsuarioEnum.Colaborador };
        _usuarioRepo.Setup(r => r.GetByIdAsync(usuarioId, default)).ReturnsAsync(usuario);

        var dto = new UpdateUsuarioDto("Nome", null, null, PerfilUsuarioEnum.AdminEmpresa);
        await Assert.ThrowsAsync<UnauthorizedAccessException>(
            () => _handler.Handle(new UpdateUsuarioCommand(usuarioId, EmpresaId, false, dto), default));
    }

    [Fact]
    public async Task Handle_DevePermitir_QuandoSuperAdminPromoveParaAdmin()
    {
        var usuarioId = Guid.NewGuid();
        var usuario = new Usuario { EmpresaId = EmpresaId, Perfil = PerfilUsuarioEnum.Colaborador };
        _usuarioRepo.Setup(r => r.GetByIdAsync(usuarioId, default)).ReturnsAsync(usuario);

        var dto = new UpdateUsuarioDto("Nome", null, null, PerfilUsuarioEnum.AdminEmpresa);
        var result = await _handler.Handle(new UpdateUsuarioCommand(usuarioId, EmpresaId, true, dto), default);

        result.Should().BeTrue();
        usuario.Perfil.Should().Be(PerfilUsuarioEnum.AdminEmpresa);
    }
}
