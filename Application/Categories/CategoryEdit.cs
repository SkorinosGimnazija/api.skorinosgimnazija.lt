namespace Application.Categories;

using AutoMapper;
using Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;
using System.Diagnostics.CodeAnalysis;

public static class CategoryEdit
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

        [SuppressMessage("ReSharper", "MethodSupportsCancellation")]
        public async Task<bool> Handle(Command request, CancellationToken _)
        {
            var entity = await _context.Categories.FirstOrDefaultAsync(x => x.Id == request.Category.Id);
            if (entity is null)
            {
                return false;
            }

            _mapper.Map(request.Category, entity);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}