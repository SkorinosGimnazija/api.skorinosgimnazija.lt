namespace SkorinosGimnazija.Application.Accomplishments;

using System.Diagnostics.CodeAnalysis;
using AutoMapper;
using Common.Interfaces;
using Domain.Entities.Accomplishments;
using Dtos;
using FluentValidation;
using MediatR;
using Validators;

public static class AccomplishmentCreate
{
    public record Command(AccomplishmentCreateDto Accomplishment) : IRequest<AccomplishmentDto>;

    public class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(v => v.Accomplishment).NotNull().SetValidator(new AccomplishmentCreateValidator());
        }
    }

    public class Handler : IRequestHandler<Command, AccomplishmentDto>
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
        public async Task<AccomplishmentDto> Handle(Command request, CancellationToken _)
        {
            var entity = _context.Accomplishments.Add(_mapper.Map<Accomplishment>(request.Accomplishment)).Entity;

            entity.UserId = _currentUser.UserId;

            await _context.SaveChangesAsync();

            return _mapper.Map<AccomplishmentDto>(entity);
        }
    }
}