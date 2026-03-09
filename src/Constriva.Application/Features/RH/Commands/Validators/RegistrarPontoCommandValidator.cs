using FluentValidation;

namespace Constriva.Application.Features.RH.Commands;

public class RegistrarPontoCommandValidator : AbstractValidator<RegistrarPontoCommand>
{
    public RegistrarPontoCommandValidator()
    {
        RuleFor(x => x.Dto.FuncionarioId).NotEqual(Guid.Empty)
            .WithMessage("FuncionarioId é obrigatório.");
    }
}
