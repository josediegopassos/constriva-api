using Constriva.Application.Features.RH.Commands;
using Constriva.Application.Features.RH.DTOs;
using Constriva.Domain.Enums;
using FluentAssertions;
using Xunit;

namespace Constriva.Tests.Features.RH;

public class RegistrarPontoValidatorTests
{
    private readonly RegistrarPontoCommandValidator _validator = new();

    [Fact]
    public void Validate_DevePassar_ComDadosValidos()
    {
        var cmd = new RegistrarPontoCommand(Guid.NewGuid(),
            new RegistrarPontoDto(Guid.NewGuid(), null, TipoRegistroPontoEnum.Entrada, DateTime.Now, "08:00", null, null, null, null));
        _validator.Validate(cmd).IsValid.Should().BeTrue();
    }

    [Fact]
    public void Validate_DeveFalhar_QuandoFuncionarioNaoInformado()
    {
        var cmd = new RegistrarPontoCommand(Guid.NewGuid(),
            new RegistrarPontoDto(Guid.Empty, null, TipoRegistroPontoEnum.Entrada, DateTime.Now, "08:00", null, null, null, null));
        var result = _validator.Validate(cmd);
        result.IsValid.Should().BeFalse();
    }
}
