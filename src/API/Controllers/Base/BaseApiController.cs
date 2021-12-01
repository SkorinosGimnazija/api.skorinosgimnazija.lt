namespace SkorinosGimnazija.API.Controllers.Base;

using System.Net.Mime;
using MediatR;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("[controller]")]
[Produces(MediaTypeNames.Application.Json)]
public abstract class BaseApiController : ControllerBase
{
    private IMediator? _mediator;

    protected IMediator Mediator
    {
        get { return _mediator ??= HttpContext.RequestServices.GetRequiredService<IMediator>(); }
    }
}