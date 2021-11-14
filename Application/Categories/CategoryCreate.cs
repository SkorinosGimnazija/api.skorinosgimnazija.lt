namespace Application.Categories;

using AutoMapper;
using Domain.CMS;
using Dtos;
using MediatR;
using Persistence;

public static class CategoryCreate
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
    }
}