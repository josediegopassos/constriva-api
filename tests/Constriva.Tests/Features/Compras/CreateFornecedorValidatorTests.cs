using Constriva.Application.Features.Compras.Commands;
using Constriva.Application.Features.Compras.DTOs;
using Constriva.Domain.Enums;
using FluentAssertions;
using Xunit;

namespace Constriva.Tests.Features.Compras;

public class CreateFornecedorValidatorTests
{
    private readonly CreateFornecedorCommandValidator _validator = new();

    [Fact]
    public void Validate_DevePassar_ComDadosPJValidos()
    {
        var cmd = new CreateFornecedorCommand(Guid.NewGuid(),
            new CreateFornecedorDto("Construtora Silva LTDA", "Construtora Silva",
                "12.345.678/0001-99", null, TipoFornecedorEnum.Material,
                "(11) 99999-9999", "contato@silva.com.br", "São Paulo", "SP"));

        var result = _validator.Validate(cmd);
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Validate_DeveFalhar_QuandoRazaoSocialVazia()
    {
        var cmd = new CreateFornecedorCommand(Guid.NewGuid(),
            new CreateFornecedorDto("", null, "12.345.678/0001-99",
                null, TipoFornecedorEnum.Material, null, "email@test.com", null, null));

        var result = _validator.Validate(cmd);
        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public void Validate_DeveFalhar_QuandoDocumentoVazio()
    {
        var cmd = new CreateFornecedorCommand(Guid.NewGuid(),
            new CreateFornecedorDto("Empresa Teste", null, "",
                null, TipoFornecedorEnum.Material, null, "email@test.com", null, null));

        var result = _validator.Validate(cmd);
        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public void Validate_DeveFalhar_QuandoEmailInvalido()
    {
        var cmd = new CreateFornecedorCommand(Guid.NewGuid(),
            new CreateFornecedorDto("Empresa Teste", null, "12345678000199",
                null, TipoFornecedorEnum.Material, null, "emailinvalido", null, null));

        var result = _validator.Validate(cmd);
        result.IsValid.Should().BeFalse();
    }
}
