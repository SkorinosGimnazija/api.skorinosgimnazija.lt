namespace Application.Posts
{
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
        public record Command(MenuEditDto Menu) : IRequest<bool>;

        public class Handler : IRequestHandler<Command, bool>
        {
            private readonly DataContext _context;

            private readonly IMapper _mapper;

            public Handler(DataContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<bool> Handle(Command request, CancellationToken cancellationToken)
            {
                var entity = await _context.Menus.FirstOrDefaultAsync(x => x.Id == request.Menu.Id, cancellationToken);
                if (entity is null)
                {
                    return false;
                }

                _mapper.Map(request.Menu, entity);
                await _context.SaveChangesAsync(cancellationToken);

                return true;
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