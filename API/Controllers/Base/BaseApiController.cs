namespace API.Controllers.Base
{
    using MediatR;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.DependencyInjection;

    [ApiController]
    public class BaseApiController : ControllerBase
    {
        private IMediator? _mediator;

        protected IMediator Mediator
        {
            get { return _mediator ??= HttpContext.RequestServices.GetService<IMediator>()!; }
        }
    }
}