using FluentValidation;

namespace Constriva.Application.Features.RH.Commands;

public class CreateFuncionarioCommandValidator : AbstractValidator<CreateFuncionarioCommand>
{
    public CreateFuncionarioCommandValidator()
    {
        RuleFor(x => x.Dto.Nome).NotEmpty().WithMessage("Nome é obrigatório.");
        RuleFor(x => x.Dto.SalarioBase).GreaterThanOrEqualTo(0).WithMessage("SalarioBase não pode ser negativo.");
        When(x => !string.IsNullOrEmpty(x.Dto.Email),
            () => RuleFor(x => x.Dto.Email).EmailAddress().WithMessage("E-mail inválido."));
    }
}
