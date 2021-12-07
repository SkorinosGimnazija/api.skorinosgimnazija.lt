namespace SkorinosGimnazija.Application.Menus;

using System.Diagnostics.CodeAnalysis;
using AutoMapper;
using Common.Interfaces;
using Domain.Entities;
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
            RuleFor(v => v.Menu).SetValidator(new MenuCreateValidator());
        }
    }

    public class Handler : IRequestHandler<Command, MenuDto>
    {
        private readonly IAppDbContext _context;
        private readonly ISearchClient _searchClient;
        private readonly IMapper _mapper;

        public Handler(IAppDbContext context, ISearchClient searchClient, IMapper mapper)
        {
            _context = context;
            _searchClient = searchClient;
            _mapper = mapper;
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

            return _mapper.Map<MenuDto>(entity);
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
            entity.Path = parentMenu.Path + entity.Path;
        }
    }
}