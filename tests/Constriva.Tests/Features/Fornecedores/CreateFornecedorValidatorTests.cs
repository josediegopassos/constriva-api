using Constriva.Application.Features.Fornecedores.Commands;
using Constriva.Application.Features.Fornecedores.DTOs;
using Constriva.Application.Features.Fornecedores.Validators;
using Constriva.Domain.Enums;
using FluentAssertions;
using Xunit;

namespace Constriva.Tests.Features.Fornecedores;

public class CreateFornecedorValidatorTests
{
    private readonly CreateFornecedorCommandValidator _validator = new();

    [Fact]
    public void Validate_DevePassar_ComDadosPJValidos()
    {
        var cmd = new CreateFornecedorCommand(Guid.NewGuid(), Guid.NewGuid(),
            new CreateFornecedorDto(TipoPessoaEnum.PessoaJuridica, "Construtora Silva LTDA",
                "12.345.678/0001-99", "contato@silva.com.br", TipoFornecedorEnum.Material,
                NomeFantasia: "Construtora Silva", Telefone: "(11) 99999-9999",
                Cidade: "São Paulo", Estado: "SP"));

        var result = _validator.Validate(cmd);
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Validate_DeveFalhar_QuandoRazaoSocialVazia()
    {
        var cmd = new CreateFornecedorCommand(Guid.NewGuid(), Guid.NewGuid(),
            new CreateFornecedorDto(TipoPessoaEnum.PessoaJuridica, "",
                "12.345.678/0001-99", "email@test.com", TipoFornecedorEnum.Material));

        var result = _validator.Validate(cmd);
        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public void Validate_DeveFalhar_QuandoDocumentoVazio()
    {
        var cmd = new CreateFornecedorCommand(Guid.NewGuid(), Guid.NewGuid(),
            new CreateFornecedorDto(TipoPessoaEnum.PessoaJuridica, "Empresa Teste",
                "", "email@test.com", TipoFornecedorEnum.Material));

        var result = _validator.Validate(cmd);
        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public void Validate_DeveFalhar_QuandoEmailInvalido()
    {
        var cmd = new CreateFornecedorCommand(Guid.NewGuid(), Guid.NewGuid(),
            new CreateFornecedorDto(TipoPessoaEnum.PessoaJuridica, "Empresa Teste",
                "12345678000199", "emailinvalido", TipoFornecedorEnum.Material));

        var result = _validator.Validate(cmd);
        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public void Validate_DeveFalhar_QuandoEstadoInvalido()
    {
        var cmd = new CreateFornecedorCommand(Guid.NewGuid(), Guid.NewGuid(),
            new CreateFornecedorDto(TipoPessoaEnum.PessoaJuridica, "Empresa Teste",
                "12345678000199", "email@test.com", TipoFornecedorEnum.Material,
                Estado: "sao paulo"));

        var result = _validator.Validate(cmd);
        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public void Validate_DeveFalhar_QuandoCepInvalido()
    {
        var cmd = new CreateFornecedorCommand(Guid.NewGuid(), Guid.NewGuid(),
            new CreateFornecedorDto(TipoPessoaEnum.PessoaJuridica, "Empresa Teste",
                "12345678000199", "email@test.com", TipoFornecedorEnum.Material,
                Cep: "123"));

        var result = _validator.Validate(cmd);
        result.IsValid.Should().BeFalse();
    }
}
