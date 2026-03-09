using FluentValidation;

namespace Constriva.Application.Features.Empresas.Commands;

public class CreateEmpresaCommandValidator : AbstractValidator<CreateEmpresaCommand>
{
    public CreateEmpresaCommandValidator()
    {
        RuleFor(x => x.Dto.RazaoSocial).NotEmpty().WithMessage("Razão social é obrigatória.");
        RuleFor(x => x.Dto.Cnpj).NotEmpty().Matches(@"^\d{2}\.\d{3}\.\d{3}/\d{4}-\d{2}$")
            .WithMessage("Cnpj inválido.");
        RuleFor(x => x.Dto.Email).NotEmpty().EmailAddress().WithMessage("E-mail inválido.");
        RuleFor(x => x.Dto.EmailAdmin).NotEmpty().EmailAddress().WithMessage("E-mail do admin inválido.");
    }
}
