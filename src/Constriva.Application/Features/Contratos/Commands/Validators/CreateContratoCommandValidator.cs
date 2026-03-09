using FluentValidation;

namespace Constriva.Application.Features.Contratos.Commands;

public class CreateContratoCommandValidator : AbstractValidator<CreateContratoCommand>
{
    public CreateContratoCommandValidator()
    {
        RuleFor(x => x.Dto.Objeto).NotEmpty().WithMessage("Objeto do contrato é obrigatório.");
        RuleFor(x => x.Dto.ValorGlobal).GreaterThan(0).WithMessage("ValorGlobal deve ser maior que zero.");
    }
}
