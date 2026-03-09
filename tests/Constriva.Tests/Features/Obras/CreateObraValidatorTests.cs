using Constriva.Application.Features.Obras.Commands;
using Constriva.Application.Features.Obras.DTOs;
using Constriva.Application.Features.Obras.Validators;
using Constriva.Domain.Enums;
using FluentAssertions;
using Xunit;

namespace Constriva.Tests.Features.Obras;

public class CreateObraValidatorTests
{
    private readonly CreateObraCommandValidator _validator = new();

    [Fact]
    public void Validate_DevePassar_ComDadosCompletos()
    {
        var cmd = new CreateObraCommand(Guid.NewGuid(), Guid.NewGuid(), new CreateObraDto(
            "Obra Valida", TipoObraEnum.Residencial, TipoContratoObraEnum.Empreitada,
            null, null,
            DateTime.Today, DateTime.Today.AddDays(100), 500_000m,
            "Endereço", "1", null, "Bairro",
            "Cidade", "SP", "01310-100"));

        _validator.Validate(cmd).IsValid.Should().BeTrue();
    }

    [Fact]
    public void Validate_DeveFalhar_QuandoNomeVazio()
    {
        var cmd = new CreateObraCommand(Guid.NewGuid(), Guid.NewGuid(), new CreateObraDto(
            "", TipoObraEnum.Residencial, TipoContratoObraEnum.Empreitada,
            null, null,
            DateTime.Today, DateTime.Today.AddDays(100), 500_000m,
            "Endereço", "1", null, "Bairro",
            "Cidade", "SP", "01310-100"));

        var result = _validator.Validate(cmd);
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName.Contains("Nome"));
    }

    [Fact]
    public void Validate_DeveFalhar_QuandoDataFimAnteriorAInicio()
    {
        var cmd = new CreateObraCommand(Guid.NewGuid(), Guid.NewGuid(), new CreateObraDto(
            "Obra", TipoObraEnum.Residencial, TipoContratoObraEnum.Empreitada,
            null, null,
            DateTime.Today.AddDays(10), DateTime.Today, 500_000m,
            "Endereço", "1", null, "Bairro",
            "Cidade", "SP", "01310-100"));

        var result = _validator.Validate(cmd);
        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public void Validate_DeveFalhar_QuandoValorPrevistaNegativo()
    {
        var cmd = new CreateObraCommand(Guid.NewGuid(), Guid.NewGuid(), new CreateObraDto(
            "Obra", TipoObraEnum.Residencial, TipoContratoObraEnum.Empreitada,
            null, null,
            DateTime.Today, DateTime.Today.AddDays(100), -1m,
            "Endereço", "1", null, "Bairro",
            "Cidade", "SP", "01310-100"));

        var result = _validator.Validate(cmd);
        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public void Validate_DeveFalhar_QuandoEnderecoVazio()
    {
        var cmd = new CreateObraCommand(Guid.NewGuid(), Guid.NewGuid(), new CreateObraDto(
            "Obra", TipoObraEnum.Residencial, TipoContratoObraEnum.Empreitada,
            null, null,
            DateTime.Today, DateTime.Today.AddDays(100), 500_000m,
            "", "1", null, "Bairro",
            "Cidade", "SP", "01310-100"));

        var result = _validator.Validate(cmd);
        result.IsValid.Should().BeFalse();
    }
}
