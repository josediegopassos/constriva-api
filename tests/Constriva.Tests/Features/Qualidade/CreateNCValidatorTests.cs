using Constriva.Application.Features.Qualidade.Commands;
using Constriva.Application.Features.Qualidade.DTOs;
using Constriva.Domain.Enums;
using FluentAssertions;
using Xunit;

namespace Constriva.Tests.Features.Qualidade;

public class CreateNCValidatorTests
{
    private readonly CreateNCCommandValidator _validator = new();

    [Fact]
    public void Validate_DeveFalhar_QuandoDescricaoVazia()
    {
        var cmd = new CreateNCCommand(Guid.NewGuid(), new CreateNCDto(
            Guid.NewGuid(), "NC-001", "Título NC", "",
            GravidadeNCEnum.Baixa, "Qualidade", null, null));
        var result = _validator.Validate(cmd);
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName.Contains("Descricao"));
    }

    [Fact]
    public void Validate_DevePassar_ComDadosValidos()
    {
        var cmd = new CreateNCCommand(Guid.NewGuid(), new CreateNCDto(
            Guid.NewGuid(), "NC-001", "Título NC",
            "Concreto fora do traço especificado",
            GravidadeNCEnum.Alta, "Concretagem", null, null));
        _validator.Validate(cmd).IsValid.Should().BeTrue();
    }
}
