using Constriva.Application.Features.Estoque.Commands;
using Constriva.Application.Features.Estoque.DTOs;
using FluentAssertions;
using Xunit;

namespace Constriva.Tests.Features.Estoque;

public class CreateRequisicaoValidatorTests
{
    private readonly CreateRequisicaoValidator _validator = new();

    [Fact]
    public void Validate_DevePassar_ComDadosValidos()
    {
        var cmd = new CreateRequisicaoCommand(Guid.NewGuid(), Guid.NewGuid(),
            new CreateRequisicaoDto(Guid.NewGuid(), Guid.NewGuid(), "Material para fundação", null, null, null));

        var result = _validator.Validate(cmd);
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Validate_DeveFalhar_QuandoMotivoVazio()
    {
        var cmd = new CreateRequisicaoCommand(Guid.NewGuid(), Guid.NewGuid(),
            new CreateRequisicaoDto(Guid.NewGuid(), Guid.NewGuid(), "", null, null, null));

        var result = _validator.Validate(cmd);
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName.Contains("Motivo"));
    }

    [Fact]
    public void Validate_DeveFalhar_QuandoObraIdVazio()
    {
        var cmd = new CreateRequisicaoCommand(Guid.NewGuid(), Guid.NewGuid(),
            new CreateRequisicaoDto(Guid.Empty, Guid.NewGuid(), "Motivo válido", null, null, null));

        var result = _validator.Validate(cmd);
        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public void Validate_DeveFalhar_QuandoMotivoMuitoLongo()
    {
        var motivo = new string('A', 501);
        var cmd = new CreateRequisicaoCommand(Guid.NewGuid(), Guid.NewGuid(),
            new CreateRequisicaoDto(Guid.NewGuid(), Guid.NewGuid(), motivo, null, null, null));

        var result = _validator.Validate(cmd);
        result.IsValid.Should().BeFalse();
    }
}
