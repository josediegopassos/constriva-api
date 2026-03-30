using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Constriva.Application.Common.Interfaces;
using Constriva.Application.Features.RH;

namespace Constriva.API.Controllers;

[Authorize]
[Route("api/v1/bancos")]
public sealed class BancosController : BaseController
{
    public BancosController(IMediator mediator, ICurrentUser currentUser)
        : base(mediator, currentUser) { }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<BancoDto>>> GetBancos(CancellationToken ct)
        => Ok(await Mediator.Send(new GetBancosQuery(), ct));
}
