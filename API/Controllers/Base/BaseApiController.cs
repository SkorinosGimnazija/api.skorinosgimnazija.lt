namespace API.Controllers.Base;

using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

[ApiController]
[Route("[controller]")]
[Produces(MediaTypeNames.Application.Json)]
public class BaseApiController : ControllerBase
{
    private IMediator? _mediator;

    protected IMediator Mediator
    {
        get { return _mediator ??= HttpContext.RequestServices.GetService<IMediator>()!; }
    }
}