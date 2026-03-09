using Constriva.Application.Features.Obras.DTOs;
using FluentValidation;
using Constriva.Application.Features.Obras.Commands;
using Constriva.Domain.Enums;

namespace Constriva.Application.Features.Obras.Validators;

public class CreateObraCommandValidator : AbstractValidator<CreateObraCommand>
{
    public CreateObraCommandValidator()
    {
        RuleFor(x => x.Dto.Nome).NotEmpty().MaximumLength(200).WithMessage("Nome da obra é obrigatório.");
        RuleFor(x => x.Dto.ValorContrato).GreaterThan(0).WithMessage("Valor do contrato deve ser positivo.");
        RuleFor(x => x.Dto.DataFimPrevista)
            .GreaterThan(x => x.Dto.DataInicioPrevista)
            .WithMessage("Data de término deve ser posterior à data de início.");
        RuleFor(x => x.Dto.Estado).NotEmpty().Length(2).Matches("^[A-Z]{2}$")
            .WithMessage("Estado deve ser a sigla com 2 letras maiúsculas.");
        RuleFor(x => x.Dto.Cep).NotEmpty().Matches(@"^\d{5}-?\d{3}$")
            .WithMessage("CEP inválido.");
        RuleFor(x => x.Dto.Cidade).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Dto.Logradouro).NotEmpty().MaximumLength(300);
    }
}

public class UpdateObraCommandValidator : AbstractValidator<UpdateObraCommand>
{
    public UpdateObraCommandValidator()
    {
        RuleFor(x => x.Dto.Nome).NotEmpty().MaximumLength(200).When(x => x.Dto.Nome != null);
        RuleFor(x => x.Dto.ValorContrato).GreaterThan(0).When(x => x.Dto.ValorContrato.HasValue);
        RuleFor(x => x.Dto.DataFimPrevista)
            .GreaterThan(x => x.Dto.DataInicioPrevista)
            .When(x => x.Dto.DataFimPrevista.HasValue && x.Dto.DataInicioPrevista.HasValue);
    }
}

public class UpdateStatusObraCommandValidator : AbstractValidator<UpdateStatusObraCommand>
{
    public UpdateStatusObraCommandValidator()
    {
        RuleFor(x => x.Status).IsInEnum();
        RuleFor(x => x.Observacao).NotEmpty()
            .When(x => x.Status == StatusObraEnum.Paralisada || x.Status == StatusObraEnum.Cancelada)
            .WithMessage("Observação é obrigatória para obras paralisadas ou canceladas.");
    }
}

public class UpdatePercentualObraCommandValidator : AbstractValidator<UpdatePercentualObraCommand>
{
    public UpdatePercentualObraCommandValidator()
    {
        RuleFor(x => x.Percentual).InclusiveBetween(0, 100)
            .WithMessage("Percentual deve estar entre 0 e 100.");
    }
}

public class CreateFaseCommandValidator : AbstractValidator<CreateFaseCommand>
{
    public CreateFaseCommandValidator()
    {
        RuleFor(x => x.Nome).NotEmpty().MaximumLength(200);
        RuleFor(x => x.ValorPrevisto).GreaterThanOrEqualTo(0);
        RuleFor(x => x.Fim).GreaterThan(x => x.Inicio)
            .WithMessage("Data de término da fase deve ser posterior ao início.");
    }
}
