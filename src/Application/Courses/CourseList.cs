namespace SkorinosGimnazija.Application.Courses;

using AutoMapper;
using AutoMapper.QueryableExtensions;
using Common.Extensions;
using Common.Interfaces;
using Dtos;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SkorinosGimnazija.Application.Common.Pagination;
using SkorinosGimnazija.Application.Posts;

public static class CourseList
{
    public record Query(PaginationDto Pagination) : IRequest<PaginatedList<CourseDto>>;

    public class Validator : AbstractValidator<Query>
    {
        public Validator()
        {
            RuleFor(x => x.Pagination).NotNull().SetValidator(new PaginationValidator());
        }
    }

    public class Handler : IRequestHandler<Query, PaginatedList<CourseDto>>
    {
        private readonly IAppDbContext _context;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUser;

        public Handler(IAppDbContext context, IMapper mapper, ICurrentUserService currentUser)
        {
            _context = context;
            _mapper = mapper;
            _currentUser = currentUser;
        }

        public async Task<PaginatedList<CourseDto>> Handle(Query request, CancellationToken cancellationToken)
        {
            return await _context.Courses
                       .AsNoTracking()
                       .Where(x => x.UserId == _currentUser.UserId)
                       .ProjectTo<CourseDto>(_mapper.ConfigurationProvider)
                       .OrderByDescending(x => x.Id)
                       .ToPaginatedListAsync(request.Pagination, cancellationToken);
        }
    }
}