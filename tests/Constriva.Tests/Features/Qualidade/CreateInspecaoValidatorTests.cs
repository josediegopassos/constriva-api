using Constriva.Application.Features.Qualidade.Commands;
using Constriva.Application.Features.Qualidade.DTOs;
using FluentAssertions;
using Xunit;

namespace Constriva.Tests.Features.Qualidade;

public class CreateInspecaoValidatorTests
{
    private readonly CreateInspecaoCommandValidator _validator = new();

    [Fact]
    public void Validate_DevePassar_ComDadosValidos()
    {
        var cmd = new CreateInspecaoCommand(Guid.NewGuid(), new CreateInspecaoDto(
            Guid.NewGuid(), "INS-001", "Inspeção Pré-Serviço", null,
            DateTime.Today.AddDays(7), null, null));
        _validator.Validate(cmd).IsValid.Should().BeTrue();
    }

    [Fact]
    public void Validate_DeveFalhar_QuandoNumeroVazio()
    {
        var cmd = new CreateInspecaoCommand(Guid.NewGuid(), new CreateInspecaoDto(
            Guid.NewGuid(), "", "Título", null, DateTime.Today, null, null));
        var result = _validator.Validate(cmd);
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName.Contains("Numero"));
    }

    [Fact]
    public void Validate_DeveFalhar_QuandoDataProgramadaNoPassado()
    {
        var cmd = new CreateInspecaoCommand(Guid.NewGuid(), new CreateInspecaoDto(
            Guid.NewGuid(), "INS-001", "Inspeção Rotina", null,
            DateTime.Today.AddDays(-1), null, null));
        var result = _validator.Validate(cmd);
        result.IsValid.Should().BeFalse();
    }
}
