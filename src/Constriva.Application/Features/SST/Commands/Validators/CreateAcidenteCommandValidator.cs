using FluentValidation;

namespace Constriva.Application.Features.SST.Commands;

public class CreateAcidenteCommandValidator : AbstractValidator<CreateAcidenteCommand>
{
    public CreateAcidenteCommandValidator()
    {
        RuleFor(x => x.Dto.DescricaoAcidente).NotEmpty().WithMessage("Descrição do acidente é obrigatória.");
        When(x => x.Dto.AfastamentoMedico, () =>
            RuleFor(x => x.Dto.DiasAfastamento).NotNull().GreaterThan(0)
                .WithMessage("DiasAfastamento é obrigatório quando há afastamento médico."));
    }
}
