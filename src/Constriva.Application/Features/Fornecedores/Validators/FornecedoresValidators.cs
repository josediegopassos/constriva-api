using FluentValidation;
using Constriva.Application.Features.Fornecedores.Commands;

namespace Constriva.Application.Features.Fornecedores.Validators;

public class CreateFornecedorCommandValidator : AbstractValidator<CreateFornecedorCommand>
{
    public CreateFornecedorCommandValidator()
    {
        RuleFor(x => x.Dto.RazaoSocial).NotEmpty().MaximumLength(200)
            .WithMessage("Razão Social é obrigatória.");
        RuleFor(x => x.Dto.Documento).NotEmpty().MaximumLength(18)
            .WithMessage("Documento (CPF/CNPJ) é obrigatório.");
        RuleFor(x => x.Dto.Email).NotEmpty().EmailAddress().MaximumLength(200)
            .WithMessage("E-mail inválido.");
        RuleFor(x => x.Dto.Estado).Length(2).Matches("^[A-Z]{2}$")
            .WithMessage("Estado deve ser a sigla com 2 letras maiúsculas.")
            .When(x => x.Dto.Estado != null);
        RuleFor(x => x.Dto.Cep).Matches(@"^\d{5}-?\d{3}$")
            .WithMessage("CEP inválido.")
            .When(x => x.Dto.Cep != null);
    }
}

public class UpdateFornecedorCommandValidator : AbstractValidator<UpdateFornecedorCommand>
{
    public UpdateFornecedorCommandValidator()
    {
        RuleFor(x => x.Dto.RazaoSocial).NotEmpty().MaximumLength(200)
            .When(x => x.Dto.RazaoSocial != null);
        RuleFor(x => x.Dto.Email).EmailAddress().MaximumLength(200)
            .When(x => x.Dto.Email != null)
            .WithMessage("E-mail inválido.");
        RuleFor(x => x.Dto.Estado).Length(2).Matches("^[A-Z]{2}$")
            .WithMessage("Estado deve ser a sigla com 2 letras maiúsculas.")
            .When(x => x.Dto.Estado != null);
        RuleFor(x => x.Dto.Cep).Matches(@"^\d{5}-?\d{3}$")
            .WithMessage("CEP inválido.")
            .When(x => x.Dto.Cep != null);
    }
}
