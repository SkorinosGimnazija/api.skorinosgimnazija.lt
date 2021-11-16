namespace Application.Categories;

using AutoMapper;
using Domain.CMS;
using Dtos;
using MediatR;
using Persistence;
using System.Diagnostics.CodeAnalysis;

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

        [SuppressMessage("ReSharper", "MethodSupportsCancellation")]
        public async Task<CategoryDto> Handle(Command request, CancellationToken _)
        {
            var entity = _context.Categories.Add(_mapper.Map<Category>(request.Category)).Entity;
            await _context.SaveChangesAsync();

            return _mapper.Map<CategoryDto>(entity);
        }
    }
}