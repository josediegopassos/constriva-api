using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Constriva.Application.Common.Interfaces;
using Constriva.Application.Features.Agente.Exceptions;

namespace Constriva.API.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
[Produces("application/json")]
public abstract class BaseController : ControllerBase
{
    protected readonly IMediator Mediator;
    protected readonly ICurrentUser CurrentUser;

    protected BaseController(IMediator mediator, ICurrentUser currentUser)
    {
        Mediator = mediator;
        CurrentUser = currentUser;
    }

    protected ActionResult<T> OkOrNotFound<T>(T? result) =>
        result is null ? NotFound() : Ok(result);

    protected Guid RequireEmpresaId() =>
        CurrentUser.EmpresaId ?? throw new UnauthorizedAccessException("EmpresaId não encontrado no contexto.");

    protected ActionResult HandleException(Exception ex) => ex switch
    {
        ValidationException ve => BadRequest(new
        {
            message = "Validação falhou.",
            errors = ve.Errors.Select(e => new { field = e.PropertyName, error = e.ErrorMessage })
        }),
        UnauthorizedAccessException => Forbid(),
        KeyNotFoundException => NotFound(new { message = ex.Message }),
        InvalidOperationException => BadRequest(new { message = ex.Message }),
        CotaExcedidaException cota => StatusCode(402, new { message = cota.Message, percentualUsado = cota.PercentualUsado, tokensRestantes = cota.TokensRestantes }),
        DbUpdateException dbe => Conflict(new { message = ExtractDbErrorMessage(dbe), detail = dbe.ToString() }),
        _ => StatusCode(500, new { message = ex.Message, type = ex.GetType().Name, detail = ex.ToString() })
    };

    private static string ExtractDbErrorMessage(DbUpdateException ex)
    {
        var inner = ex.InnerException?.Message ?? ex.Message;
        if (inner.Contains("23505") || inner.Contains("unique constraint") || inner.Contains("duplicate key"))
            return $"Já existe um registro com esses dados (violação de unicidade). Detalhe: {inner}";
        if (inner.Contains("23503") || inner.Contains("foreign key"))
            return $"Operação inválida: existe um vínculo com outro registro. Detalhe: {inner}";
        return $"Erro ao salvar os dados. Detalhe: {inner}";
    }
}
