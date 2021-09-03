namespace Application.Posts
{
    using System.Threading;
    using System.Threading.Tasks;
    using Application.Menus.Dtos;
    using AutoMapper;
    using Categories.Dtos;
    using Categories.Validation;
    using Domain.CMS;
    using Dtos;
    using FluentValidation;
    using MediatR;
    using Menus.Validation;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Persistence;
    using Validation;

    public class CategoryEdit
    {
        public record Command(CategoryEditDto Category) : IRequest<bool>;

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
                var entity = await _context.Categories.FirstOrDefaultAsync(x => x.Id == request.Category.Id, cancellationToken);
                if (entity is null)
                {
                    return false;
                }

                _mapper.Map(request.Category, entity);
                await _context.SaveChangesAsync(cancellationToken);

                return true;
            }

            public class CommandValidator : AbstractValidator<Command>
            {
                public CommandValidator()
                {
                    RuleFor(x => x.Category).SetValidator(new CategoryEditValidator());
                }
            }
        }
    }
}