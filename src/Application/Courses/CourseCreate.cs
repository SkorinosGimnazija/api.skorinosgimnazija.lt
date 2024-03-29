﻿namespace SkorinosGimnazija.Application.Courses;

using System.Diagnostics.CodeAnalysis;
using AutoMapper;
using Common.Interfaces;
using Domain.Entities.Courses;
using Dtos;
using FluentValidation;
using MediatR;
using Validators;

public static class CourseCreate
{
    public record Command(CourseCreateDto Course) : IRequest<CourseDto>;

    public class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(v => v.Course).NotNull().SetValidator(new CourseCreateValidator());
        }
    }

    public class Handler : IRequestHandler<Command, CourseDto>
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

        [SuppressMessage("ReSharper", "MethodSupportsCancellation")]
        public async Task<CourseDto> Handle(Command request, CancellationToken _)
        {
            var entity = _context.Courses.Add(_mapper.Map<Course>(request.Course)).Entity;

            entity.UserId = _currentUser.UserId;

            await _context.SaveChangesAsync();

            return _mapper.Map<CourseDto>(entity);
        }
    }
}