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
    using Categories.Dtos;
    using Categories.Validation;
    using Dtos;
    using Domain = Domain.CMS.Domain;

    public class CategoryCreate
    {
        public record Command(CategoryCreateDto Category) : IRequest<ActionResult<Category>>;

        public class Handler : IRequestHandler<Command, ActionResult<Category>>
        {
            private readonly DataContext _context;
            private readonly IMapper _mapper;

            public Handler(DataContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<ActionResult<Category>> Handle(Command request, CancellationToken cancellationToken)
            {
                var entity = _mapper.Map(request.Category, new Category());

                _context.Categories.Add(entity);
                await _context.SaveChangesAsync(cancellationToken);

                return new OkObjectResult(entity) { StatusCode = StatusCodes.Status201Created };
            }

            public class CommandValidator : AbstractValidator<Command>
            {
                public CommandValidator()
                {
                    RuleFor(x => x.Category).SetValidator(new CategoryCreateValidator());
                }
            }
        }
    }
}