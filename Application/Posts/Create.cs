namespace Application.Posts
{
    using System.Threading;
    using System.Threading.Tasks;
    using Domain.CMS;
    using FluentValidation;
    using FluentValidation.Validators;
    using MediatR;
    using Persistence;

    public class Create
    {
        public class Handler : IRequestHandler<Command>
        {
            private readonly DataContext _context;

            public Handler(DataContext context)
            {
                _context = context;
            }

            public class CommandValidator :AbstractValidator<Command>
            {
                public CommandValidator()
                {
                    RuleFor(x => x.Post).SetValidator(new PostValidator());
                }
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                _context.Posts.Add(request.Post);
                await _context.SaveChangesAsync(cancellationToken);

                return Unit.Value;
            }
        }

        public record Command(Post Post) : IRequest;
    }
}