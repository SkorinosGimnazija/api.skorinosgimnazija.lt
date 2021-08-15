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

    public class DomainDetails
    {
        public record Query(int Id) : IRequest<ActionResult<Domain>>;
        public class Handler : IRequestHandler<Query, ActionResult<Domain>>
        {
            private readonly DataContext _context;
            private readonly IMapper _mapper;

            public Handler(DataContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<ActionResult<Domain>> Handle(Query request, CancellationToken cancellationToken)
            {
                var domain = await _context.Domains.ProjectTo<Domain>(_mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

                if (domain == null)
                {
                    return new NotFoundResult();
                }

                return domain;
            }
        }

    }
}