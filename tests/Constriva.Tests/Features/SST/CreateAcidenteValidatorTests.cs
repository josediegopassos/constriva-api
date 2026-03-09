using Constriva.Application.Features.SST.Commands;
using Constriva.Application.Features.SST.DTOs;
using Constriva.Domain.Enums;
using FluentAssertions;
using Xunit;

namespace Constriva.Tests.Features.SST;

public class CreateAcidenteValidatorTests
{
    private readonly CreateAcidenteCommandValidator _validator = new();

    [Fact]
    public void Validate_DevePassar_QuandoAfastamentoComDias()
    {
        var cmd = new CreateAcidenteCommand(Guid.NewGuid(),
            new CreateAcidenteDto(Guid.NewGuid(), TipoAcidenteEnum.ComAfastamento,
                "José Silva", "Andar 3", "Queda de andaime", true, 15, DateTime.Now));
        _validator.Validate(cmd).IsValid.Should().BeTrue();
    }

    [Fact]
    public void Validate_DeveFalhar_QuandoAfastamentoSemDias()
    {
        var cmd = new CreateAcidenteCommand(Guid.NewGuid(),
            new CreateAcidenteDto(Guid.NewGuid(), TipoAcidenteEnum.ComAfastamento,
                "José Silva", "Andar 3", "Queda", true, null, DateTime.Now));
        var result = _validator.Validate(cmd);
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName.Contains("DiasAfastamento"));
    }

    [Fact]
    public void Validate_DeveFalhar_QuandoDescricaoVazia()
    {
        var cmd = new CreateAcidenteCommand(Guid.NewGuid(),
            new CreateAcidenteDto(Guid.NewGuid(), TipoAcidenteEnum.SemAfastamento,
                "Nome", "Local", "", false, null, DateTime.Now));
        var result = _validator.Validate(cmd);
        result.IsValid.Should().BeFalse();
    }
}
