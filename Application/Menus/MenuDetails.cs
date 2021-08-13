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
        
    public class MenuDetails
    {
        public record Query(int Id) : IRequest<ActionResult<Menu>>;
        public class Handler : IRequestHandler<Query, ActionResult<Menu>>
        {
            private readonly DataContext _context;
            private readonly IMapper _mapper;

            public Handler(DataContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<ActionResult<Menu>> Handle(Query request, CancellationToken cancellationToken)
            {
                var menu = await _context.Menus.ProjectTo<Menu>(_mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

                if (menu == null)
                {
                    return new NotFoundResult();
                }

                return menu;
            }
        }

    }
}