using FluentValidation;

namespace Constriva.Application.Features.Estoque.Commands;

public class CreateRequisicaoValidator : AbstractValidator<CreateRequisicaoCommand>
{
    public CreateRequisicaoValidator()
    {
        RuleFor(x => x.Dto.ObraId).NotEqual(Guid.Empty).WithMessage("ObraId é obrigatório.");
        RuleFor(x => x.Dto.Motivo).NotEmpty().WithMessage("Motivo é obrigatório.")
            .MaximumLength(500).WithMessage("Motivo deve ter no máximo 500 caracteres.");
    }
}
