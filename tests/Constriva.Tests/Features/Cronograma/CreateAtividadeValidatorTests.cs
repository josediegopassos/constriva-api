using Constriva.Application.Features.Cronograma.Commands;
using Constriva.Application.Features.Cronograma.DTOs;
using FluentAssertions;
using Xunit;

namespace Constriva.Tests.Features.Cronograma;

public class CreateAtividadeValidatorTests
{
    private readonly CreateAtividadeCommandValidator _validator = new();

    [Fact]
    public void Validate_DevePassar_ComDadosValidos()
    {
        var cmd = new CreateAtividadeCommand(Guid.NewGuid(), Guid.NewGuid(),
            new CreateAtividadeDto("Nome Válido", null, 1, DateTime.Today,
                DateTime.Today.AddDays(30), 30m));

        _validator.Validate(cmd).IsValid.Should().BeTrue();
    }

    [Fact]
    public void Validate_DeveFalhar_ComNomeVazio()
    {
        var cmd = new CreateAtividadeCommand(Guid.NewGuid(), Guid.NewGuid(),
            new CreateAtividadeDto("", null, 1, DateTime.Today, DateTime.Today.AddDays(30), 30m));

        var result = _validator.Validate(cmd);
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName.Contains("Nome"));
    }

    [Fact]
    public void Validate_DeveFalhar_ComDataInicioMaiorQueDataFim()
    {
        var cmd = new CreateAtividadeCommand(Guid.NewGuid(), Guid.NewGuid(),
            new CreateAtividadeDto("Atividade", null, 1,
                DateTime.Today.AddDays(10), DateTime.Today, 5m));

        var result = _validator.Validate(cmd);
        result.IsValid.Should().BeFalse();
    }
}
