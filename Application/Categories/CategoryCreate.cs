using Application.Menus.Validation;
using AutoMapper;
using Domain.CMS;
using FluentValidation;

namespace Application.Domains
{
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

    public class CategoryCreate
    {
        public record Command(CategoryCreateDto Category) : IRequest<CategoryDto>;

        public class Handler : IRequestHandler<Command, CategoryDto>
        {
            private readonly DataContext _context;
            private readonly IMapper _mapper;

            public Handler(DataContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<CategoryDto> Handle(Command request, CancellationToken cancellationToken)
            {
                var entity = _mapper.Map(request.Category, new Category());

                _context.Categories.Add(entity);
                await _context.SaveChangesAsync(cancellationToken);

                return _mapper.Map(entity, new CategoryDto());
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