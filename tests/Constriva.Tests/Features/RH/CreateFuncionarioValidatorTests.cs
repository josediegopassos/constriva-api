using Constriva.Application.Features.RH.Commands;
using Constriva.Application.Features.RH.DTOs;
using Constriva.Domain.Enums;
using FluentAssertions;
using Xunit;

namespace Constriva.Tests.Features.RH;

public class CreateFuncionarioValidatorTests
{
    private readonly CreateFuncionarioCommandValidator _validator = new();

    private static CreateFuncionarioCommand ValidCmd() => new(Guid.NewGuid(),
        new CreateFuncionarioDto(
            "Carlos Eduardo Souza", "123.456.789-09",
            "carlos@email.com", "(11) 99999-0000",
            Guid.NewGuid(), Guid.NewGuid(),
            DateTime.Today.AddYears(-5), 5000m, StatusFuncionarioEnum.Ativo));

    [Fact]
    public void Validate_DevePassar_ComDadosValidos()
        => _validator.Validate(ValidCmd()).IsValid.Should().BeTrue();

    [Fact]
    public void Validate_DeveFalhar_QuandoNomeVazio()
    {
        var cmd = ValidCmd() with { Dto = ValidCmd().Dto with { Nome = "" } };
        _validator.Validate(cmd).IsValid.Should().BeFalse();
    }

    [Fact]
    public void Validate_DeveFalhar_QuandoSalarioNegativo()
    {
        var cmd = ValidCmd() with { Dto = ValidCmd().Dto with { SalarioBase = -100m } };
        var result = _validator.Validate(cmd);
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName.Contains("SalarioBase"));
    }

    [Fact]
    public void Validate_DeveFalhar_QuandoEmailInvalido()
    {
        var cmd = ValidCmd() with { Dto = ValidCmd().Dto with { Email = "email-invalido" } };
        var result = _validator.Validate(cmd);
        result.IsValid.Should().BeFalse();
    }
}
