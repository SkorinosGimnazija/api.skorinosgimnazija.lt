namespace SkorinosGimnazija.Application.Accomplishments;

using System.Diagnostics.CodeAnalysis;
using AutoMapper;
using Common.Exceptions;
using Common.Interfaces;
using Dtos;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Validators;

public static class AccomplishmentEdit
{
    public record Command(AccomplishmentEditDto Accomplishment) : IRequest<Unit>;

    public class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(v => v.Accomplishment).NotNull().SetValidator(new AccomplishmentEditValidator());
        }
    }

    public class Handler : IRequestHandler<Command, Unit>
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
        public async Task<Unit> Handle(Command request, CancellationToken _)
        {
            var entity = await _context.Accomplishments
                             .Include(x => x.Students)
                             .Include(x => x.AdditionalTeachers)
                             .FirstOrDefaultAsync(x => x.Id == request.Accomplishment.Id);

            if (entity is null)
            {
                throw new NotFoundException();
            }

            if (!_currentUser.IsOwnerOrManager(entity.UserId))
            {
                throw new UnauthorizedAccessException();
            }

            var transaction = await _context.BeginTransactionAsync();

            entity.Students.Clear();
            entity.AdditionalTeachers.Clear();

            _mapper.Map(request.Accomplishment, entity);

            await _context.SaveChangesAsync();
            await transaction.CommitAsync();

            return Unit.Value;
        }
    }
}