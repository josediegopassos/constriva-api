using Constriva.Application.Features.Financeiro.DTOs;
using FluentValidation;
using Constriva.Application.Features.Financeiro.Commands;

namespace Constriva.Application.Features.Financeiro.Validators;

public class CreateLancamentoValidator : AbstractValidator<CreateLancamentoCommand>
{
    public CreateLancamentoValidator()
    {
        RuleFor(x => x.Dto.Descricao).NotEmpty().MaximumLength(500);
        RuleFor(x => x.Dto.Valor).GreaterThan(0).WithMessage("Valor deve ser maior que zero.");
    }
}

public class BaixarLancamentoValidator : AbstractValidator<BaixarLancamentoCommand>
{
    public BaixarLancamentoValidator()
    {
        RuleFor(x => x.ValorRealizado).GreaterThan(0).WithMessage("Valor realizado deve ser maior que zero.");
        RuleFor(x => x.DataPagamento).NotEqual(default(DateTime)).WithMessage("Data de pagamento é obrigatória.");
    }
}
