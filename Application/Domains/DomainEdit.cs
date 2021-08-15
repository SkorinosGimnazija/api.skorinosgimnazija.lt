namespace Application.Posts
{
    using System.Threading;
    using System.Threading.Tasks;
    using Application.Menus.Dtos;
    using AutoMapper;
    using Domain.CMS;
    using Domains.Dtos;
    using Domains.Validation;
    using Dtos;
    using FluentValidation;
    using MediatR;
    using Menus.Validation;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Persistence;
    using Validation;

    public class DomainEdit
    {
        public record Command(Domain Domain) : IRequest<IActionResult>;

        public class Handler : IRequestHandler<Command, IActionResult>
        {
            private readonly DataContext _context;

            private readonly IMapper _mapper;

            public Handler(DataContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<IActionResult> Handle(Command request, CancellationToken cancellationToken)
            {
                var domain = await _context.Domains.FirstOrDefaultAsync(x => x.Id == request.Domain.Id, cancellationToken);
                if (domain == null)
                {
                    return new NotFoundResult();
                }

                _mapper.Map(request.Domain, domain);
                await _context.SaveChangesAsync(cancellationToken);

                return new OkResult();
            }

            public class CommandValidator : AbstractValidator<Command>
            {
                public CommandValidator()
                {
                    RuleFor(x => x.Domain).SetValidator(new DomainEditValidator());
                }
            }
        }
    }
}