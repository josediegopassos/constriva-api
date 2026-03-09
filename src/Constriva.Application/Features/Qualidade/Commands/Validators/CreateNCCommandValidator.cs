using FluentValidation;

namespace Constriva.Application.Features.Qualidade.Commands;

public class CreateNCCommandValidator : AbstractValidator<CreateNCCommand>
{
    public CreateNCCommandValidator()
    {
        RuleFor(x => x.Dto.Descricao).NotEmpty().WithMessage("Descricao é obrigatória.");
    }
}
