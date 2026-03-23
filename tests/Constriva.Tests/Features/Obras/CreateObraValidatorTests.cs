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

    private static CreateObraDto ValidDto(string? nome = null, DateTime? inicio = null, DateTime? fim = null,
        decimal? valor = null, string? logradouro = null) => new(
        nome ?? "Obra Valida", TipoObraEnum.Residencial, TipoContratoObraEnum.Empreitada,
        null, null, null,
        inicio ?? DateTime.Today, fim ?? DateTime.Today.AddDays(100),
        valor ?? 500_000m,
        logradouro ?? "Endereço", "1", null, "Bairro",
        "Cidade", "SP", "01310-100");

    [Fact]
    public void Validate_DevePassar_ComDadosCompletos()
    {
        var cmd = new CreateObraCommand(Guid.NewGuid(), Guid.NewGuid(), ValidDto());
        _validator.Validate(cmd).IsValid.Should().BeTrue();
    }

    [Fact]
    public void Validate_DeveFalhar_QuandoNomeVazio()
    {
        var cmd = new CreateObraCommand(Guid.NewGuid(), Guid.NewGuid(), ValidDto(nome: ""));
        var result = _validator.Validate(cmd);
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName.Contains("Nome"));
    }

    [Fact]
    public void Validate_DeveFalhar_QuandoDataFimAnteriorAInicio()
    {
        var cmd = new CreateObraCommand(Guid.NewGuid(), Guid.NewGuid(),
            ValidDto(inicio: DateTime.Today.AddDays(10), fim: DateTime.Today));
        var result = _validator.Validate(cmd);
        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public void Validate_DeveFalhar_QuandoValorPrevistaNegativo()
    {
        var cmd = new CreateObraCommand(Guid.NewGuid(), Guid.NewGuid(), ValidDto(valor: -1m));
        var result = _validator.Validate(cmd);
        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public void Validate_DeveFalhar_QuandoEnderecoVazio()
    {
        var cmd = new CreateObraCommand(Guid.NewGuid(), Guid.NewGuid(), ValidDto(logradouro: ""));
        var result = _validator.Validate(cmd);
        result.IsValid.Should().BeFalse();
    }
}
