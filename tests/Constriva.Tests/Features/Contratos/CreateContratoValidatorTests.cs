using Constriva.Application.Features.Contratos.Commands;
using Constriva.Application.Features.Contratos.DTOs;
using Constriva.Domain.Enums;
using FluentAssertions;
using Xunit;

namespace Constriva.Tests.Features.Contratos;

public class CreateContratoValidatorTests
{
    private readonly CreateContratoCommandValidator _validator = new();

    [Fact]
    public void Validate_DevePassar_ComDadosCompletos()
    {
        var cmd = new CreateContratoCommand(Guid.NewGuid(), new CreateContratoDto(
            Guid.NewGuid(), Guid.NewGuid(), "Objeto do contrato",
            TipoContratoFornecedorEnum.Global,
            500_000m, DateTime.Today, DateTime.Today, DateTime.Today.AddMonths(6)));

        _validator.Validate(cmd).IsValid.Should().BeTrue();
    }

    [Fact]
    public void Validate_DeveFalhar_QuandoObjetoVazio()
    {
        var cmd = new CreateContratoCommand(Guid.NewGuid(), new CreateContratoDto(
            Guid.NewGuid(), Guid.NewGuid(), "",
            TipoContratoFornecedorEnum.Global,
            100_000m, DateTime.Today, DateTime.Today, DateTime.Today.AddMonths(6)));

        var result = _validator.Validate(cmd);
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName.Contains("Objeto"));
    }

    [Fact]
    public void Validate_DeveFalhar_QuandoValorZero()
    {
        var cmd = new CreateContratoCommand(Guid.NewGuid(), new CreateContratoDto(
            Guid.NewGuid(), Guid.NewGuid(), "Objeto",
            TipoContratoFornecedorEnum.Global,
            0m, DateTime.Today, DateTime.Today, DateTime.Today.AddMonths(6)));

        var result = _validator.Validate(cmd);
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName.Contains("Valor"));
    }
}
