namespace Application.Posts
{
    using System.Threading;
    using System.Threading.Tasks;
    using Application.Menus.Dtos;
    using AutoMapper;
    using Categories.Dtos;
    using Categories.Validation;
    using Domain.CMS;
    using Domains.Dtos;
    using Domains.Validation;
    using Dtos;
    using FluentValidation;
    using MediatR;
    using Menus.Validation;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Persistence;
    using Validation;

    public class CategoryEdit
    {
        public record Command(CategoryEditDto Category) : IRequest<IActionResult>;

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
                var entity = await _context.Categories.FirstOrDefaultAsync(x => x.Id == request.Category.Id, cancellationToken);
                if (entity == null)
                {
                    return new NotFoundResult();
                }

                _mapper.Map(request.Category, entity);
                await _context.SaveChangesAsync(cancellationToken);

                return new OkResult();
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