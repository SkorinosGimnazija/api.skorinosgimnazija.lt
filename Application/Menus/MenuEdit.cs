namespace Application.Posts
{
    using System.Threading;
    using System.Threading.Tasks;
using Application.Menus.Dtos;
    using AutoMapper;
    using Domain.CMS;
    using Dtos;
    using FluentValidation;
    using MediatR;
    using Menus.Validation;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Persistence;
    using Validation;

    public class MenuEdit
    {
        public record Command(MenuEditDto Menu) : IRequest<IActionResult>;

        public class Handler : IRequestHandler<Command, IActionResult>
        {
            private readonly DataContext _context;

            private readonly IMapper _mapper;

            public Handler(DataContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<IActionResult> Handle(Command request, CancellationToken cancellationToken)
            {
                var menu = await _context.Menus.FirstOrDefaultAsync(x => x.Id == request.Menu.Id, cancellationToken);
                if (menu == null)
                {
                    return new NotFoundResult();
                }

                _mapper.Map(request.Menu, menu);
                await _context.SaveChangesAsync(cancellationToken);

                return new OkResult();
            }

            public class CommandValidator : AbstractValidator<Command>
            {
                public CommandValidator()
                {
                    RuleFor(x => x.Menu).SetValidator(new MenuEditValidator());
                }
            }
        }
    }
}