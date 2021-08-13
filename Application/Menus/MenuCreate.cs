namespace Application.Posts
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
using Application.Menus.Validation;
    using AutoMapper;
    using Domain.CMS;
    using Dtos;
    using FluentValidation;
    using MediatR;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Persistence;
    using Validation;

    public class MenuCreate
    {
        public record Command(MenuCreateDto Menu) : IRequest<ActionResult<Menu>>;

        public class Handler : IRequestHandler<Command, ActionResult<Menu>>
        {
            private readonly DataContext _context;
            private readonly IMapper _mapper;

            public Handler(DataContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<ActionResult<Menu>> Handle(Command request, CancellationToken cancellationToken)
            {
                var menu = _mapper.Map(request.Menu, new Menu());

                _context.Menus.Add(menu);
                await _context.SaveChangesAsync(cancellationToken);

                return new OkObjectResult(menu) { StatusCode = StatusCodes.Status201Created };
            }

            public class CommandValidator : AbstractValidator<Command>
            {
                public CommandValidator()
                {
                    RuleFor(x => x.Menu).SetValidator(new MenuCreateValidator());
                }
            }
        }
    }
}