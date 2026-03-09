using Constriva.Application.Features.Usuarios.Commands;
using Constriva.Application.Features.Usuarios.DTOs;
using Constriva.Domain.Enums;
using FluentAssertions;
using Xunit;

namespace Constriva.Tests.Features.Usuarios;

public class CreateUsuarioValidatorTests
{
    private readonly CreateUsuarioValidator _validator = new();

    [Fact]
    public void Validate_DevePassar_ComDadosValidos()
    {
        var cmd = new CreateUsuarioCommand(false, Guid.NewGuid(), new CreateUsuarioDto(
            "Nome Completo", "email@empresa.com", "Senha@123!", null, null, PerfilUsuarioEnum.AdminEmpresa, Guid.NewGuid()));
        _validator.Validate(cmd).IsValid.Should().BeTrue();
    }

    [Fact]
    public void Validate_DeveFalhar_ComEmailInvalido()
    {
        var cmd = new CreateUsuarioCommand(false, Guid.NewGuid(), new CreateUsuarioDto(
            "Nome", "email-invalido", "Senha@123!", null, null, PerfilUsuarioEnum.AdminEmpresa, Guid.NewGuid()));
        var result = _validator.Validate(cmd);
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName.Contains("Email"));
    }

    [Fact]
    public void Validate_DeveFalhar_ComNomeVazio()
    {
        var cmd = new CreateUsuarioCommand(false, Guid.NewGuid(), new CreateUsuarioDto(
            "", "email@empresa.com", "Senha@123!", null, null, PerfilUsuarioEnum.AdminEmpresa, Guid.NewGuid()));
        _validator.Validate(cmd).IsValid.Should().BeFalse();
    }

    [Fact]
    public void Validate_DeveFalhar_ComSenhaMuitoCurta()
    {
        var cmd = new CreateUsuarioCommand(false, Guid.NewGuid(), new CreateUsuarioDto(
            "Nome", "email@empresa.com", "123", null, null, PerfilUsuarioEnum.AdminEmpresa, Guid.NewGuid()));
        var result = _validator.Validate(cmd);
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName.Contains("Senha"));
    }
}
