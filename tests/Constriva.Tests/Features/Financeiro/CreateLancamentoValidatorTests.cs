using Constriva.Application.Features.Financeiro.Commands;
using Constriva.Application.Features.Financeiro.DTOs;
using Constriva.Application.Features.Financeiro.Validators;
using Constriva.Domain.Enums;
using FluentAssertions;
using Xunit;

namespace Constriva.Tests.Features.Financeiro;

public class CreateLancamentoValidatorTests
{
    private readonly CreateLancamentoValidator _validator = new();

    [Fact]
    public void Validate_DevePassar_ComDadosValidos()
    {
        var cmd = new CreateLancamentoCommand(Guid.NewGuid(), Guid.NewGuid(),
            new CreateLancamentoDto(TipoLancamentoEnum.Despesa, "Pagamento fornecedor",
                5000m, DateTime.Today.AddDays(30),
                null, null, null, null, FormaPagamentoEnum.Boleto, "NF-001", "NF-001", "Fornecedores"));

        var result = _validator.Validate(cmd);
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Validate_DeveFalhar_QuandoDescricaoVazia()
    {
        var cmd = new CreateLancamentoCommand(Guid.NewGuid(), Guid.NewGuid(),
            new CreateLancamentoDto(TipoLancamentoEnum.Despesa, "",
                5000m, DateTime.Today, null, null, null, null, null, null, null, null));

        var result = _validator.Validate(cmd);
        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public void Validate_DeveFalhar_QuandoValorZero()
    {
        var cmd = new CreateLancamentoCommand(Guid.NewGuid(), Guid.NewGuid(),
            new CreateLancamentoDto(TipoLancamentoEnum.Despesa, "Descricao",
                0m, DateTime.Today, null, null, null, null, null, null, null, null));

        var result = _validator.Validate(cmd);
        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public void Validate_DeveFalhar_QuandoValorNegativo()
    {
        var cmd = new CreateLancamentoCommand(Guid.NewGuid(), Guid.NewGuid(),
            new CreateLancamentoDto(TipoLancamentoEnum.Receita, "Descricao",
                -1000m, DateTime.Today, null, null, null, null, null, null, null, null));

        var result = _validator.Validate(cmd);
        result.IsValid.Should().BeFalse();
    }
}
