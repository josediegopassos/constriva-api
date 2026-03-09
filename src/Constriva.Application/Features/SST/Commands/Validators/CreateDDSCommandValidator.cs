using FluentValidation;

namespace Constriva.Application.Features.SST.Commands;

public class CreateDDSCommandValidator : AbstractValidator<CreateDDSCommand>
{
    public CreateDDSCommandValidator()
    {
        RuleFor(x => x.Dto.Tema).NotEmpty().WithMessage("Tema é obrigatório.");
        RuleFor(x => x.Dto.NumeroParticipantes).GreaterThan(0)
            .WithMessage("NumeroParticipantes deve ser maior que zero.");
    }
}
