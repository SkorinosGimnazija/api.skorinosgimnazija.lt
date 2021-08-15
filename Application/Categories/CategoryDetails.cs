namespace Application.Posts2
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using Domain.CMS;
    using MediatR;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Persistence;

    public class CategoryDetails
    {
        public record Query(int Id) : IRequest<ActionResult<Category>>;
        public class Handler : IRequestHandler<Query, ActionResult<Category>>
        {
            private readonly DataContext _context;
            private readonly IMapper _mapper;

            public Handler(DataContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<ActionResult<Category>> Handle(Query request, CancellationToken cancellationToken)
            {
                var entity = await _context.Categories.ProjectTo<Category>(_mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

                if (entity == null)
                {
                    return new NotFoundResult();
                }

                return entity;
            }
        }

    }
}