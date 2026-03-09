using FluentValidation;
using Constriva.Application.Features.Orcamento.Commands;
using Constriva.Application.Features.Orcamento.DTOs;

namespace Constriva.Application.Features.Orcamento.Validators;

// ─── Orçamento Validators ──────────────────────────────────────────────────────

public class CreateOrcamentoValidator : AbstractValidator<CreateOrcamentoCommand>
{
    public CreateOrcamentoValidator()
    {
        RuleFor(x => x.ObraId)
            .NotEmpty().WithMessage("ObraId é obrigatório.");

        RuleFor(x => x.EmpresaId)
            .NotEmpty().WithMessage("EmpresaId é obrigatório.");

        RuleFor(x => x.Dto.Nome)
            .NotEmpty().WithMessage("Nome do orçamento é obrigatório.")
            .MaximumLength(200).WithMessage("Nome deve ter no máximo 200 caracteres.");

        RuleFor(x => x.Dto.BDI)
            .GreaterThanOrEqualTo(0).WithMessage("BDI não pode ser negativo.")
            .LessThan(100).WithMessage("BDI deve ser menor que 100%.");

        RuleFor(x => x.Dto.DataReferencia)
            .NotEmpty().WithMessage("Data de referência é obrigatória.")
            .LessThanOrEqualTo(DateTime.Today.AddDays(30)).WithMessage("Data de referência não pode ser muito futura.");

        RuleFor(x => x.Dto.BDIDetalhadoAdministracao)
            .GreaterThanOrEqualTo(0).WithMessage("BDI Administração não pode ser negativo.");

        RuleFor(x => x.Dto.BDIDetalhadoSeguro)
            .GreaterThanOrEqualTo(0).WithMessage("BDI Seguro não pode ser negativo.");

        RuleFor(x => x.Dto.BDIDetalhadoRisco)
            .GreaterThanOrEqualTo(0).WithMessage("BDI Risco não pode ser negativo.");

        RuleFor(x => x.Dto.BDIDetalhadoFinanceiro)
            .GreaterThanOrEqualTo(0).WithMessage("BDI Financeiro não pode ser negativo.");

        RuleFor(x => x.Dto.BDIDetalhadoLucro)
            .GreaterThanOrEqualTo(0).WithMessage("BDI Lucro não pode ser negativo.");

        RuleFor(x => x.Dto.BDIDetalhadoTributos)
            .GreaterThanOrEqualTo(0).WithMessage("BDI Tributos não pode ser negativo.");

        RuleFor(x => x.Dto.EncargosHoristas)
            .GreaterThanOrEqualTo(0).WithMessage("Encargos Horistas não pode ser negativo.")
            .LessThanOrEqualTo(200).WithMessage("Encargos Horistas deve ser no máximo 200%.");

        RuleFor(x => x.Dto.EncargosMensalistas)
            .GreaterThanOrEqualTo(0).WithMessage("Encargos Mensalistas não pode ser negativo.")
            .LessThanOrEqualTo(200).WithMessage("Encargos Mensalistas deve ser no máximo 200%.");

        RuleFor(x => x.Dto.Localidade)
            .MaximumLength(100).WithMessage("Localidade deve ter no máximo 100 caracteres.")
            .When(x => x.Dto.Localidade != null);

        RuleFor(x => x.Dto.BaseOrcamentaria)
            .MaximumLength(100).WithMessage("Base orçamentária deve ter no máximo 100 caracteres.")
            .When(x => x.Dto.BaseOrcamentaria != null);
    }
}

public class UpdateOrcamentoValidator : AbstractValidator<UpdateOrcamentoCommand>
{
    public UpdateOrcamentoValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Id do orçamento é obrigatório.");

        RuleFor(x => x.EmpresaId)
            .NotEmpty().WithMessage("EmpresaId é obrigatório.");

        RuleFor(x => x.Dto.Nome)
            .NotEmpty().WithMessage("Nome do orçamento é obrigatório.")
            .MaximumLength(200).WithMessage("Nome deve ter no máximo 200 caracteres.");

        RuleFor(x => x.Dto.BDI)
            .GreaterThanOrEqualTo(0).WithMessage("BDI não pode ser negativo.")
            .LessThan(100).WithMessage("BDI deve ser menor que 100%.");

        RuleFor(x => x.Dto.DataReferencia)
            .NotEmpty().WithMessage("Data de referência é obrigatória.");
    }
}

public class AprovarOrcamentoValidator : AbstractValidator<AprovarOrcamentoCommand>
{
    public AprovarOrcamentoValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Id do orçamento é obrigatório.");

        RuleFor(x => x.EmpresaId)
            .NotEmpty().WithMessage("EmpresaId é obrigatório.");

        RuleFor(x => x.UsuarioId)
            .NotEmpty().WithMessage("UsuarioId é obrigatório.");

        RuleFor(x => x.Observacao)
            .MaximumLength(1000).WithMessage("Observação deve ter no máximo 1000 caracteres.")
            .When(x => x.Observacao != null);
    }
}

public class ReprovarOrcamentoValidator : AbstractValidator<ReprovarOrcamentoCommand>
{
    public ReprovarOrcamentoValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Id do orçamento é obrigatório.");

        RuleFor(x => x.Motivo)
            .NotEmpty().WithMessage("Motivo da reprovação é obrigatório.")
            .MaximumLength(1000).WithMessage("Motivo deve ter no máximo 1000 caracteres.");
    }
}

// ─── Grupo Validators ─────────────────────────────────────────────────────────

public class CreateGrupoOrcamentoValidator : AbstractValidator<CreateGrupoOrcamentoCommand>
{
    public CreateGrupoOrcamentoValidator()
    {
        RuleFor(x => x.OrcamentoId)
            .NotEmpty().WithMessage("OrcamentoId é obrigatório.");

        RuleFor(x => x.EmpresaId)
            .NotEmpty().WithMessage("EmpresaId é obrigatório.");

        RuleFor(x => x.Dto.Nome)
            .NotEmpty().WithMessage("Nome do grupo é obrigatório.")
            .MaximumLength(200).WithMessage("Nome deve ter no máximo 200 caracteres.");
    }
}

public class UpdateGrupoOrcamentoValidator : AbstractValidator<UpdateGrupoOrcamentoCommand>
{
    public UpdateGrupoOrcamentoValidator()
    {
        RuleFor(x => x.GrupoId)
            .NotEmpty().WithMessage("GrupoId é obrigatório.");

        RuleFor(x => x.Dto.Nome)
            .NotEmpty().WithMessage("Nome do grupo é obrigatório.")
            .MaximumLength(200).WithMessage("Nome deve ter no máximo 200 caracteres.");

        RuleFor(x => x.Dto.Ordem)
            .GreaterThanOrEqualTo(1).WithMessage("Ordem deve ser maior que zero.");
    }
}

// ─── Item Validators ──────────────────────────────────────────────────────────

public class CreateItemOrcamentoValidator : AbstractValidator<CreateItemOrcamentoCommand>
{
    public CreateItemOrcamentoValidator()
    {
        RuleFor(x => x.GrupoId)
            .NotEmpty().WithMessage("GrupoId é obrigatório.");

        RuleFor(x => x.EmpresaId)
            .NotEmpty().WithMessage("EmpresaId é obrigatório.");

        RuleFor(x => x.Dto.Descricao)
            .NotEmpty().WithMessage("Descrição do item é obrigatória.")
            .MaximumLength(500).WithMessage("Descrição deve ter no máximo 500 caracteres.");

        RuleFor(x => x.Dto.UnidadeMedida)
            .NotEmpty().WithMessage("Unidade de medida é obrigatória.")
            .MaximumLength(20).WithMessage("Unidade de medida deve ter no máximo 20 caracteres.");

        RuleFor(x => x.Dto.Quantidade)
            .GreaterThan(0).WithMessage("Quantidade deve ser maior que zero.");

        RuleFor(x => x.Dto.CustoUnitario)
            .GreaterThanOrEqualTo(0).WithMessage("Custo unitário não pode ser negativo.");

        RuleFor(x => x.Dto.CodigoFonte)
            .MaximumLength(50).WithMessage("Código fonte deve ter no máximo 50 caracteres.")
            .When(x => x.Dto.CodigoFonte != null);
    }
}

public class UpdateItemOrcamentoValidator : AbstractValidator<UpdateItemOrcamentoCommand>
{
    public UpdateItemOrcamentoValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Id do item é obrigatório.");

        RuleFor(x => x.Dto.Descricao)
            .NotEmpty().WithMessage("Descrição do item é obrigatória.")
            .MaximumLength(500).WithMessage("Descrição deve ter no máximo 500 caracteres.");

        RuleFor(x => x.Dto.UnidadeMedida)
            .NotEmpty().WithMessage("Unidade de medida é obrigatória.")
            .MaximumLength(20).WithMessage("Unidade de medida deve ter no máximo 20 caracteres.");

        RuleFor(x => x.Dto.Quantidade)
            .GreaterThan(0).WithMessage("Quantidade deve ser maior que zero.");

        RuleFor(x => x.Dto.CustoUnitario)
            .GreaterThanOrEqualTo(0).WithMessage("Custo unitário não pode ser negativo.");
    }
}

// ─── Composição Validators ────────────────────────────────────────────────────

public class CreateComposicaoValidator : AbstractValidator<CreateComposicaoCommand>
{
    public CreateComposicaoValidator()
    {
        RuleFor(x => x.OrcamentoId)
            .NotEmpty().WithMessage("OrcamentoId é obrigatório.");

        RuleFor(x => x.Dto.Codigo)
            .NotEmpty().WithMessage("Código da composição é obrigatório.")
            .MaximumLength(50).WithMessage("Código deve ter no máximo 50 caracteres.");

        RuleFor(x => x.Dto.Descricao)
            .NotEmpty().WithMessage("Descrição da composição é obrigatória.")
            .MaximumLength(500).WithMessage("Descrição deve ter no máximo 500 caracteres.");

        RuleFor(x => x.Dto.UnidadeMedida)
            .NotEmpty().WithMessage("Unidade de medida é obrigatória.")
            .MaximumLength(20).WithMessage("Unidade de medida deve ter no máximo 20 caracteres.");
    }
}

public class CreateInsumoValidator : AbstractValidator<CreateInsumoCommand>
{
    public CreateInsumoValidator()
    {
        RuleFor(x => x.ComposicaoId)
            .NotEmpty().WithMessage("ComposicaoId é obrigatório.");

        RuleFor(x => x.Dto.Codigo)
            .NotEmpty().WithMessage("Código do insumo é obrigatório.")
            .MaximumLength(50).WithMessage("Código deve ter no máximo 50 caracteres.");

        RuleFor(x => x.Dto.Descricao)
            .NotEmpty().WithMessage("Descrição do insumo é obrigatória.")
            .MaximumLength(300).WithMessage("Descrição deve ter no máximo 300 caracteres.");

        RuleFor(x => x.Dto.UnidadeMedida)
            .NotEmpty().WithMessage("Unidade de medida é obrigatória.")
            .MaximumLength(20).WithMessage("Unidade de medida deve ter no máximo 20 caracteres.");

        RuleFor(x => x.Dto.Coeficiente)
            .GreaterThan(0).WithMessage("Coeficiente deve ser maior que zero.");

        RuleFor(x => x.Dto.PrecoUnitario)
            .GreaterThanOrEqualTo(0).WithMessage("Preço unitário não pode ser negativo.");
    }
}

// ─── SINAPI Validator ─────────────────────────────────────────────────────────

public class ImportarSinapiValidator : AbstractValidator<ImportarSinapiCommand>
{
    public ImportarSinapiValidator()
    {
        RuleFor(x => x.OrcamentoId)
            .NotEmpty().WithMessage("OrcamentoId é obrigatório.");

        RuleFor(x => x.EmpresaId)
            .NotEmpty().WithMessage("EmpresaId é obrigatório.");

        RuleFor(x => x.Dto.Itens)
            .NotEmpty().WithMessage("Lista de itens SINAPI não pode ser vazia.")
            .When(x => x.Dto.Itens != null);

        RuleForEach(x => x.Dto.Itens!).SetValidator(new ItemSinapiValidator())
            .When(x => x.Dto.Itens != null && x.Dto.Itens.Any());
    }
}

public class ItemSinapiValidator : AbstractValidator<ItemSinapiDto>
{
    public ItemSinapiValidator()
    {
        RuleFor(x => x.Codigo)
            .NotEmpty().WithMessage("Código SINAPI é obrigatório.");

        RuleFor(x => x.Descricao)
            .NotEmpty().WithMessage("Descrição do item SINAPI é obrigatória.")
            .MaximumLength(500).WithMessage("Descrição deve ter no máximo 500 caracteres.");

        RuleFor(x => x.UnidadeMedida)
            .NotEmpty().WithMessage("Unidade de medida é obrigatória.");

        RuleFor(x => x.CustoUnitario)
            .GreaterThan(0).WithMessage("Custo unitário SINAPI deve ser maior que zero.");

        RuleFor(x => x.Quantidade)
            .GreaterThan(0).WithMessage("Quantidade deve ser maior que zero.");
    }
}
