using Constriva.Application.Features.Financeiro.Commands;
using Constriva.Application.Features.Financeiro.Validators;
using FluentAssertions;
using Xunit;

namespace Constriva.Tests.Features.Financeiro;

public class BaixarLancamentoValidatorTests
{
    private readonly BaixarLancamentoValidator _validator = new();

    [Fact]
    public void Validate_DevePassar_ComValorPositivo()
    {
        var cmd = new BaixarLancamentoCommand(Guid.NewGuid(), Guid.NewGuid(), 500m, DateTime.Today);
        var result = _validator.Validate(cmd);
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Validate_DeveFalhar_QuandoValorZero()
    {
        var cmd = new BaixarLancamentoCommand(Guid.NewGuid(), Guid.NewGuid(), 0m, DateTime.Today);
        var result = _validator.Validate(cmd);
        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public void Validate_DeveFalhar_QuandoDataNaoInformada()
    {
        var cmd = new BaixarLancamentoCommand(Guid.NewGuid(), Guid.NewGuid(), 100m, default);
        var result = _validator.Validate(cmd);
        result.IsValid.Should().BeFalse();
    }
}
