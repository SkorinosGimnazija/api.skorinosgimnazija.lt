namespace Application.Posts
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using Domain.CMS;
    using Dtos;
    using MediatR;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Persistence;

    public class PublicPostDetails
    {
        public record Query(int Id) : IRequest<ActionResult<PublicPostDetailsDto>>;
        public class Handler : IRequestHandler<Query, ActionResult<PublicPostDetailsDto>>
        {
            private readonly DataContext _context;
            private readonly IMapper _mapper;

            public Handler(DataContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<ActionResult<PublicPostDetailsDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var post = await _context.Posts.ProjectTo<PublicPostDetailsDto>(_mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

                if (post == null)
                {
                    return new NotFoundResult();
                }
                 
                return post;
            }
        }

    }
}