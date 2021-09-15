namespace Application.Posts2
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
using Application.Categories.Dtos;
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using Domain.CMS;
    using MediatR;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Persistence;

    public class CategoryDetails
    {
        public record Query(int Id) : IRequest<CategoryDto?>;
        public class Handler : IRequestHandler<Query, CategoryDto?>
        {
            private readonly DataContext _context;
            private readonly IMapper _mapper;

            public Handler(DataContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<CategoryDto?> Handle(Query request, CancellationToken cancellationToken)
            {
                return await _context.Categories
                    .AsNoTracking()
                    .ProjectTo<CategoryDto>(_mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
            }
        }

    }
}