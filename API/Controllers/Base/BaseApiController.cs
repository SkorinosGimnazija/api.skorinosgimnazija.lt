namespace API.Controllers.Base;

using System.Net.Mime;
using MediatR;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Produces(MediaTypeNames.Application.Json)]
public class BaseApiController : ControllerBase
{
    private IMediator? _mediator;

    protected IMediator Mediator
    {
        get { return _mediator ??= HttpContext.RequestServices.GetService<IMediator>()!; }
    }
}