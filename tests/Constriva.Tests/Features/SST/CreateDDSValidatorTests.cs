using Constriva.Application.Features.SST.Commands;
using Constriva.Application.Features.SST.DTOs;
using Constriva.Domain.Enums;
using FluentAssertions;
using Xunit;

namespace Constriva.Tests.Features.SST;

public class CreateDDSValidatorTests
{
    private readonly CreateDDSCommandValidator _validator = new();

    [Fact]
    public void Validate_DevePassar_ComDadosValidos()
    {
        var cmd = new CreateDDSCommand(Guid.NewGuid(), Guid.NewGuid(),
            new CreateDDSDto(Guid.NewGuid(), "DDS-001", "Trabalho em altura", null, "João", 15, DateTime.Today));
        _validator.Validate(cmd).IsValid.Should().BeTrue();
    }

    [Fact]
    public void Validate_DeveFalhar_QuandoTemaVazio()
    {
        var cmd = new CreateDDSCommand(Guid.NewGuid(), Guid.NewGuid(),
            new CreateDDSDto(Guid.NewGuid(), "DDS-001", "", null, null, 10, DateTime.Today));
        var result = _validator.Validate(cmd);
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName.Contains("Tema"));
    }

    [Fact]
    public void Validate_DeveFalhar_QuandoParticipantesZero()
    {
        var cmd = new CreateDDSCommand(Guid.NewGuid(), Guid.NewGuid(),
            new CreateDDSDto(Guid.NewGuid(), "DDS-001", "Tema", null, null, 0, DateTime.Today));
        var result = _validator.Validate(cmd);
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName.Contains("NumeroParticipantes"));
    }
}
