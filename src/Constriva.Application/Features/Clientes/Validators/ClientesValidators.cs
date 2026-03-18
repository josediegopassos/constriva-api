using FluentValidation;
using Constriva.Application.Features.Clientes.Commands;

namespace Constriva.Application.Features.Clientes.Validators;

public class CreateClienteCommandValidator : AbstractValidator<CreateClienteCommand>
{
    public CreateClienteCommandValidator()
    {
        RuleFor(x => x.Dto.Nome).NotEmpty().MaximumLength(200)
            .WithMessage("Nome do cliente é obrigatório.");
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

public class UpdateClienteCommandValidator : AbstractValidator<UpdateClienteCommand>
{
    public UpdateClienteCommandValidator()
    {
        RuleFor(x => x.Dto.Nome).NotEmpty().MaximumLength(200)
            .When(x => x.Dto.Nome != null);
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
