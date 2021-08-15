using Application.Menus.Validation;
using AutoMapper;
using Domain.CMS;
using FluentValidation;

namespace Application.Domains
{
using Application.Domains.Validation;
using MediatR;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Persistence;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
using Dtos;
using Domain = Domain.CMS.Domain;

    public class DomainCreate  
    {
        public record Command(DomainCreateDto Domain) : IRequest<ActionResult<Domain>>;

        public class Handler : IRequestHandler<Command, ActionResult<Domain>>
        {
            private readonly DataContext _context;
            private readonly IMapper _mapper;

            public Handler(DataContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<ActionResult<Domain>> Handle(Command request, CancellationToken cancellationToken)
            {
                var domain = _mapper.Map(request.Domain, new Domain());

                _context.Domains.Add(domain);
                await _context.SaveChangesAsync(cancellationToken);

                return new OkObjectResult(domain) { StatusCode = StatusCodes.Status201Created };
            }

            public class CommandValidator : AbstractValidator<Command>
            {
                public CommandValidator()
                {
                    RuleFor(x => x.Domain).SetValidator(new DomainCreateValidator());
                }
            }
        }
    }
}
