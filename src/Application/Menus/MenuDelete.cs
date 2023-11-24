namespace SkorinosGimnazija.Application.Menus;

using System.Diagnostics.CodeAnalysis;
using Common.Exceptions;
using Common.Interfaces;
using Domain.Entities.CMS;
using MediatR;
using Microsoft.EntityFrameworkCore;

public static class MenuDelete
{
    public record Command(int Id) : IRequest<Unit>;

    public class Handler : IRequestHandler<Command, Unit>
    {
        private readonly IAppDbContext _context;
        private readonly IRevalidationService _revalidation;
        private readonly ISearchClient _searchClient;

        public Handler(
            IAppDbContext context,
            ISearchClient searchClient,
            IRevalidationService revalidation)
        {
            _context = context;
            _searchClient = searchClient;
            _revalidation = revalidation;
        }

        [SuppressMessage("ReSharper", "MethodSupportsCancellation")]
        public async Task<Unit> Handle(Command request, CancellationToken _)
        {
            var entity = await _context.Menus.FirstOrDefaultAsync(x => x.Id == request.Id);
            if (entity is null)
            {
                throw new NotFoundException();
            }

            await _searchClient.RemoveMenuAsync(entity);
            _context.Menus.Remove(entity);
            await _context.SaveChangesAsync();

            await RevalidatePost(entity);

            return Unit.Value;
        }

        private async Task RevalidatePost(Menu entity)
        {
            var language = await _context.Languages.FirstAsync(x => x.Id == entity.LanguageId);
            await _revalidation.RevalidateAsync(language.Slug);
        }
    }
}