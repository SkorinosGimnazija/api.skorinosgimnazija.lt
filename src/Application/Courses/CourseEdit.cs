namespace SkorinosGimnazija.Application.Courses;

using System.Diagnostics.CodeAnalysis;
using AutoMapper;
using Common.Exceptions;
using Common.Interfaces;
using Dtos;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SkorinosGimnazija.Application.Menus.Dtos;
using SkorinosGimnazija.Domain.Entities;
using Validators;

public static class CourseEdit
{
    public record Command(CourseEditDto Course) : IRequest<Unit>;

    public class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(v => v.Course).NotNull().SetValidator(new CourseEditValidator());
        }
    }

    public class Handler : IRequestHandler<Command, Unit>
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

        [SuppressMessage("ReSharper", "MethodSupportsCancellation")]
        public async Task<Unit> Handle(Command request, CancellationToken _)
        {
            var entity = await _context.Courses.FirstOrDefaultAsync(x => x.Id == request.Course.Id);
            if (entity is null)
            {
                throw new NotFoundException();
            }

            if (!_currentUser.IsOwnerOrAdmin(entity.UserId))
            {
                throw new UnauthorizedAccessException();
            }

            _mapper.Map(request.Course, entity);
            await _context.SaveChangesAsync();

            return Unit.Value;
        }
    }
}