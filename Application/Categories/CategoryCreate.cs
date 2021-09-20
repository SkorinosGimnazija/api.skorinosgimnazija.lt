namespace Application.Categories
{
    using System.Threading;
    using System.Threading.Tasks;
    using AutoMapper;
    using Domain.CMS;
    using Dtos;
    using FluentValidation;
    using MediatR;
    using Persistence;
    using Validation;

    public class CategoryCreate
    {
        public record Command(CategoryCreateDto Category) : IRequest<CategoryDto>;

        public class Handler : IRequestHandler<Command, CategoryDto>
        {
            private readonly DataContext _context;
            private readonly IMapper _mapper;

            public Handler(DataContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<CategoryDto> Handle(Command request, CancellationToken cancellationToken)
            {
                var entity = _mapper.Map(request.Category, new Category());

                _context.Categories.Add(entity);
                await _context.SaveChangesAsync(cancellationToken);

                return _mapper.Map(entity, new CategoryDto());
            }

            public class CommandValidator : AbstractValidator<Command>
            {
                public CommandValidator()
                {
                    RuleFor(x => x.Category).SetValidator(new CategoryCreateValidator());
                }
            }
        }
    }
}