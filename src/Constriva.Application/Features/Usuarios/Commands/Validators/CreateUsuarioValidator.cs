using FluentValidation;

namespace Constriva.Application.Features.Usuarios.Commands;

public class CreateUsuarioValidator : AbstractValidator<CreateUsuarioCommand>
{
    public CreateUsuarioValidator()
    {
        RuleFor(x => x.Dto.Nome).NotEmpty().WithMessage("Nome é obrigatório.");
        RuleFor(x => x.Dto.Email).NotEmpty().EmailAddress().WithMessage("E-mail inválido.");
        RuleFor(x => x.Dto.Senha).MinimumLength(6).WithMessage("Senha deve ter pelo menos 6 caracteres.");
    }
}
