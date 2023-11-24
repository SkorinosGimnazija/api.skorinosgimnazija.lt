namespace SkorinosGimnazija.Application.Menus;

using System.Diagnostics.CodeAnalysis;
using AutoMapper;
using Common.Exceptions;
using Common.Interfaces;
using Domain.Entities.CMS;
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
            RuleFor(v => v.Menu).NotNull().SetValidator(new MenuEditValidator());
        }
    }

    public class Handler : IRequestHandler<Command, Unit>
    {
        private readonly IAppDbContext _context;
        private readonly IMapper _mapper;
        private readonly IRevalidationService _revalidation;
        private readonly ISearchClient _searchClient;

        public Handler(
            IAppDbContext context,
            ISearchClient searchClient,
            IMapper mapper,
            IRevalidationService revalidation)
        {
            _context = context;
            _searchClient = searchClient;
            _mapper = mapper;
            _revalidation = revalidation;
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
            await SaveSearchIndex(entity);

            await _context.SaveChangesAsync();
            await transaction.CommitAsync();

            await RevalidatePost(entity);

            return Unit.Value;
        }

        private async Task RevalidatePost(Menu entity)
        {
            var language = await _context.Languages.FirstAsync(x => x.Id == entity.LanguageId);
            await _revalidation.RevalidateAsync(language.Slug);
        }

        private async Task SaveSearchIndex(Menu menu)
        {
            await _searchClient.SaveMenuAsync(_mapper.Map<MenuIndexDto>(menu));
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
                        "Selected parent menu is not allowed")
                );
            }

            entity.Path = parentMenu.Path + "/" + entity.Path;
        }

        private async Task UpdateChildrenPaths(Menu entity, string oldMenuPath)
        {
            await _context.Menus
                .Where(x => x.Id != entity.Id && x.Path.StartsWith(oldMenuPath))
                .ForEachAsync(x => x.Path = entity.Path + "/" + x.Slug);
        }
    }
}