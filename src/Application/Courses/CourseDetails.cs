namespace SkorinosGimnazija.Application.Courses;

using AutoMapper;
using AutoMapper.QueryableExtensions;
using Common.Exceptions;
using Common.Interfaces;
using Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;

public static class CourseDetails
{
    public record Query(int Id) : IRequest<CourseDto>;

    public class Handler : IRequestHandler<Query, CourseDto>
    {
        private readonly IAppDbContext _context;
        private readonly ICurrentUserService _currentUser;
        private readonly IMapper _mapper;

        public Handler(IAppDbContext context, IMapper mapper, ICurrentUserService currentUser)
        {
            _context = context;
            _mapper = mapper;
            _currentUser = currentUser;
        }

        public async Task<CourseDto> Handle(Query request, CancellationToken cancellationToken)
        {
            var entity = await _context.Courses
                             .AsNoTracking()
                             .ProjectTo<CourseDto>(_mapper.ConfigurationProvider)
                             .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

            if (entity is null)
            {
                throw new NotFoundException();
            }

            if (!_currentUser.IsOwnerOrAdmin(entity.UserId))
            {
                throw new UnauthorizedAccessException();
            }

            return entity;
        }
    }
}