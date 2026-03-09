using FluentValidation;

namespace Constriva.Application.Features.Qualidade.Commands;

public class CreateInspecaoCommandValidator : AbstractValidator<CreateInspecaoCommand>
{
    public CreateInspecaoCommandValidator()
    {
        RuleFor(x => x.Dto.Numero).NotEmpty().WithMessage("Numero é obrigatório.");
        RuleFor(x => x.Dto.DataProgramada)
            .GreaterThanOrEqualTo(DateTime.Today)
            .WithMessage("Data programada não pode ser no passado.");
    }
}
