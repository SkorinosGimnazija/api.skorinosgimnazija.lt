namespace Application.Posts
{
    using System.Threading;
    using System.Threading.Tasks;
    using AutoMapper;
    using Domain.CMS;
    using Dtos;
    using FluentValidation;
    using MediatR;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Persistence;
    using Validation;

    public class PostPatch
    {
        public record Command(int Id, PostPatchDto Post) : IRequest<IActionResult>;

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
                var post = await _context.Posts.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
                if (post == null)
                {
                    return new NotFoundResult();
                }

                _mapper.Map(request.Post, post);
                await _context.SaveChangesAsync(cancellationToken);

                return new OkResult();
            }
        }
    }
}