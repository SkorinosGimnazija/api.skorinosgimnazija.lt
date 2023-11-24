namespace SkorinosGimnazija.Application.Posts;

using System.Diagnostics.CodeAnalysis;
using AutoMapper;
using Common.Exceptions;
using Common.Interfaces;
using Domain.Entities.CMS;
using Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;

public static class PostPatch
{
    public record Command(int Id, PostPatchDto Post) : IRequest<Unit>;

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
            var entity = await _context.Posts.FirstOrDefaultAsync(x => x.Id == request.Id);
            if (entity is null)
            {
                throw new NotFoundException();
            }

            _mapper.Map(request.Post, entity);

            await _searchClient.SavePostAsync(_mapper.Map<PostIndexDto>(entity));
            await _context.SaveChangesAsync();

            await RevalidatePost(entity);

            return Unit.Value;
        }

        private async Task RevalidatePost(Post entity)
        {
            var language = await _context.Languages.FirstAsync(x => x.Id == entity.LanguageId);
            await _revalidation.RevalidateAsync(language.Slug, entity.Slug, entity.Id);
        }
    }
}