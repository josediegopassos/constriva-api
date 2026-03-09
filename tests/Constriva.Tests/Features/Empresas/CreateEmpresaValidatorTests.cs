using Constriva.Application.Features.Empresas.Commands;
using Constriva.Application.Features.Empresas.DTOs;
using Constriva.Domain.Enums;
using FluentAssertions;
using Xunit;

namespace Constriva.Tests.Features.Empresas;

public class CreateEmpresaValidatorTests
{
    private readonly CreateEmpresaCommandValidator _validator = new();

    private static CreateEmpresaCommand ValidCmd() => new(new CreateEmpresaDto(
        "Construtora ABC Ltda", "ABC", "12.345.678/0001-90",
        "contato@abc.com.br", "(11) 3000-0000",
        "Rua da Construção", "500", null, "Centro", "São Paulo", "SP", "01310-100",
        PlanoEmpresaEnum.Profissional, 10, 5,
        "Admin ABC", "admin@abc.com.br", "Admin@123"));

    [Fact]
    public void Validate_DevePassar_ComDadosValidos()
        => _validator.Validate(ValidCmd()).IsValid.Should().BeTrue();

    [Fact]
    public void Validate_DeveFalhar_QuandoRazaoSocialVazia()
    {
        var cmd = ValidCmd() with { Dto = ValidCmd().Dto with { RazaoSocial = "" } };
        _validator.Validate(cmd).IsValid.Should().BeFalse();
    }

    [Fact]
    public void Validate_DeveFalhar_QuandoCnpjInvalido()
    {
        var cmd = ValidCmd() with { Dto = ValidCmd().Dto with { Cnpj = "111.111.111/0001-11" } };
        var result = _validator.Validate(cmd);
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName.Contains("Cnpj"));
    }

    [Fact]
    public void Validate_DeveFalhar_QuandoEmailInvalido()
    {
        var cmd = ValidCmd() with { Dto = ValidCmd().Dto with { Email = "email-invalido" } };
        var result = _validator.Validate(cmd);
        result.IsValid.Should().BeFalse();
    }
}
