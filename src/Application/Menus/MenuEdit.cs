namespace SkorinosGimnazija.Application.Menus;

using System.Diagnostics.CodeAnalysis;
using AutoMapper;
using Common.Exceptions;
using Common.Interfaces;
using Domain.Entities;
using Dtos;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Validators;
using ValidationException = Common.Exceptions.ValidationException;

public static class MenuEdit
{
    public record Command(MenuEditDto Menu) : IRequest<Unit>;

    public class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(v => v.Menu).SetValidator(new MenuEditValidator());
        }
    }

    public class Handler : IRequestHandler<Command, Unit>
    {
        private readonly IAppDbContext _context;

        private readonly IMapper _mapper;

        public Handler(IAppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [SuppressMessage("ReSharper", "MethodSupportsCancellation")]
        public async Task<Unit> Handle(Command request, CancellationToken _)
        {
            var entity = await _context.Menus.FirstOrDefaultAsync(x => x.Id == request.Menu.Id);
            if (entity is null)
            {
                throw new NotFoundException();
            }

            await using var transaction = await _context.BeginTransactionAsync();

            var oldMenuPath = entity.Path;
            _mapper.Map(request.Menu, entity);

            await _context.SaveChangesAsync();

            await UpdatePathFromParent(entity, oldMenuPath);
            await UpdateChildrenPaths(entity, oldMenuPath);

            await _context.SaveChangesAsync();
            await transaction.CommitAsync();

            return Unit.Value;
        }

        private async Task UpdatePathFromParent(Menu entity, string oldMenuPath)
        {
            if (entity.ParentMenuId is null)
            {
                return;
            }

            var parentMenu = await _context.Menus.AsNoTracking().FirstAsync(x => x.Id == entity.ParentMenuId);
            if (parentMenu.Path.StartsWith(oldMenuPath))
            {
                throw new ValidationException(
                    new ValidationFailure(nameof(entity.ParentMenuId),
                        "Selected parent menu is not valid")
                );
            }

            entity.Path = parentMenu.Path + entity.Path;
        }

        private async Task UpdateChildrenPaths(Menu entity, string oldMenuPath)
        {
            await _context.Menus
                .Where(x => x.Path.StartsWith(oldMenuPath))
                .ForEachAsync(x => x.Path = entity.Path + "/" + x.Slug);
        }
    }
}