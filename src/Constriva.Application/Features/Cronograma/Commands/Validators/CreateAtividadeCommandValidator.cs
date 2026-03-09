using FluentValidation;

namespace Constriva.Application.Features.Cronograma.Commands;

public class CreateAtividadeCommandValidator : AbstractValidator<CreateAtividadeCommand>
{
    public CreateAtividadeCommandValidator()
    {
        RuleFor(x => x.Dto.Nome).NotEmpty().WithMessage("Nome é obrigatório.");
        RuleFor(x => x.Dto.DataFimPrevista)
            .GreaterThan(x => x.Dto.DataInicioPrevista)
            .WithMessage("Data fim deve ser maior que data início.");
    }
}
