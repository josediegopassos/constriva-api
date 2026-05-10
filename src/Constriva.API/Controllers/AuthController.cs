using Constriva.Application.Common.Interfaces;
using Constriva.Application.Features.Auth.Commands;
using Constriva.Application.Features.Auth.DTOs;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Constriva.API.Controllers;

[Route("api/v1/auth")]
public sealed class AuthController : BaseController
{
    public AuthController(IMediator mediator, ICurrentUser currentUser)
        : base(mediator, currentUser) { }

    [HttpPost("login")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(AuthResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<AuthResponseDto>> Login(
        [FromBody] LoginCommand command,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await Mediator.Send(command, cancellationToken);
            return Ok(result);
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(new { message = ex.Message });
        }
    }

    [HttpPost("refresh")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(AuthResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<AuthResponseDto>> Refresh(
        [FromBody] RefreshTokenCommand command,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await Mediator.Send(command, cancellationToken);
            return Ok(result);
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(new { message = ex.Message });
        }
    }

    [HttpGet("me")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult Me() => Ok(new
    {
        userId = CurrentUser.UserId,
        empresaId = CurrentUser.EmpresaId,
        email = CurrentUser.Email,
        perfil = CurrentUser.Perfil,
        isSuperAdmin = CurrentUser.IsSuperAdmin,
        isAdminEmpresa = CurrentUser.IsAdminEmpresa
    });
}
