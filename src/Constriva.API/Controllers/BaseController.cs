using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Constriva.Application.Common.Interfaces;

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
        _ =>                        StatusCode(500, new { message = "Ocorreu um erro interno. Tente novamente." })
    };
}
