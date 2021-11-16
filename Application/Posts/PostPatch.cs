namespace Application.Posts;

using AutoMapper;
using Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;
using System.Diagnostics.CodeAnalysis;

public static class PostPatch
{
    public record Command(int Id, PostPatchDto Post) : IRequest<bool>;

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
            var entity = await _context.Posts.FirstOrDefaultAsync(x => x.Id == request.Id);
            if (entity is null)
            {
                return false;
            }

            _mapper.Map(request.Post, entity);

            //await _search.SavePost(_mapper.Map<PostIndexDto>(entity));
            await _context.SaveChangesAsync();

            return true;
        }
    }
}