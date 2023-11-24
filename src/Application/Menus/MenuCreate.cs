namespace SkorinosGimnazija.Application.Menus;

using System.Diagnostics.CodeAnalysis;
using AutoMapper;
using Common.Interfaces;
using Domain.Entities.CMS;
using Dtos;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Validators;

public static class MenuCreate
{
    public record Command(MenuCreateDto Menu) : IRequest<MenuDto>;

    public class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(v => v.Menu).NotNull().SetValidator(new MenuCreateValidator());
        }
    }

    public class Handler : IRequestHandler<Command, MenuDto>
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
        public async Task<MenuDto> Handle(Command request, CancellationToken _)
        {
            await using var transaction = await _context.BeginTransactionAsync();

            var entity = _context.Menus.Add(_mapper.Map<Menu>(request.Menu)).Entity;

            await _context.SaveChangesAsync();

            await UpdatePathFromParent(entity);
            await SaveSearchIndex(entity);

            await _context.SaveChangesAsync();
            await transaction.CommitAsync();

            await RevalidatePost(entity);

            return _mapper.Map<MenuDto>(entity);
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

        private async Task UpdatePathFromParent(Menu entity)
        {
            if (entity.ParentMenuId is null)
            {
                return;
            }

            var parentMenu = await _context.Menus.AsNoTracking().FirstAsync(x => x.Id == entity.ParentMenuId);

            entity.Path = parentMenu.Path + "/" + entity.Path;
        }
    }
}