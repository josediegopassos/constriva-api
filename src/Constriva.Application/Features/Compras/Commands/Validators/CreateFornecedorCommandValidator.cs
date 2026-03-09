using FluentValidation;

namespace Constriva.Application.Features.Compras.Commands;

public class CreateFornecedorCommandValidator : AbstractValidator<CreateFornecedorCommand>
{
    public CreateFornecedorCommandValidator()
    {
        RuleFor(x => x.Dto.RazaoSocial).NotEmpty().WithMessage("Razão social é obrigatória.");
        RuleFor(x => x.Dto.Cnpj ?? x.Dto.Cpf).NotEmpty().WithMessage("CNPJ ou CPF é obrigatório.");
        When(x => !string.IsNullOrEmpty(x.Dto.Email),
            () => RuleFor(x => x.Dto.Email).EmailAddress().WithMessage("E-mail inválido."));
    }
}
