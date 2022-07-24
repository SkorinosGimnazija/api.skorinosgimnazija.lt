namespace SkorinosGimnazija.Application.Posts;

using System.Diagnostics.CodeAnalysis;
using AutoMapper;
using Common.Interfaces;
using Domain.Entities;
using Domain.Entities.CMS;
using Dtos;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SkorinosGimnazija.Infrastructure.Revalidation;
using Validators;

public static class PostCreate
{
    public record Command(PostCreateDto Post) : IRequest<PostDetailsDto>;

    public class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(x => x.Post).NotNull().SetValidator(new PostCreateValidator());
        }
    }

    public class Handler : IRequestHandler<Command, PostDetailsDto>
    {
        private readonly IAppDbContext _context;
        private readonly IMapper _mapper;
        private readonly IMediaManager _mediaManager;
        private readonly IRevalidationService _revalidation;
        private readonly ISearchClient _searchClient;

        public Handler(
            IAppDbContext context,
            ISearchClient searchClient,
            IMediaManager mediaManager,
            IMapper mapper,
            IRevalidationService revalidation)
        {
            _context = context;
            _searchClient = searchClient;
            _mediaManager = mediaManager;
            _revalidation = revalidation;
            _mapper = mapper;
        }

        [SuppressMessage("ReSharper", "MethodSupportsCancellation")]
        public async Task<PostDetailsDto> Handle(Command request, CancellationToken _)
        {
            await using var transaction = await _context.BeginTransactionAsync();

            var entity = _context.Posts.Add(_mapper.Map<Post>(request.Post)).Entity;
            await _context.SaveChangesAsync();

            await SaveSearchIndex(entity);
            await SaveFeaturedImage(entity, request);
            await SaveImages(entity, request);
            await SaveFiles(entity, request);

            await _context.SaveChangesAsync();
            await transaction.CommitAsync();

            await RevalidatePost(entity);

            return _mapper.Map<PostDetailsDto>(entity);
        }

        private async Task RevalidatePost(Post entity)
        {
            var language = await _context.Languages.FirstAsync(x => x.Id == entity.LanguageId);
            await _revalidation.RevalidateAsync(language.Slug);
        }

        private async Task SaveSearchIndex(Post entity)
        {
            await _searchClient.SavePostAsync(_mapper.Map<PostIndexDto>(entity));
        }

        private async Task SaveImages(Post entity, Command request)
        {
            if (request.Post.NewImages?.Any() != true)
            {
                return;
            }

            try
            {
                entity.Images =
                    await _mediaManager.SaveImagesAsync(request.Post.NewImages, request.Post.OptimizeImages, false);
            }
            catch
            {
                await Cleanup(entity);
                throw;
            }
        }

        private async Task SaveFeaturedImage(Post entity, Command request)
        {
            if (request.Post.NewFeaturedImage is null)
            {
                return;
            }

            try
            {
                entity.FeaturedImage = (await _mediaManager.SaveImagesAsync(
                                            new[] { request.Post.NewFeaturedImage },
                                            request.Post.OptimizeImages, true))[0];
            }
            catch
            {
                await Cleanup(entity);
                throw;
            }
        }

        private async Task SaveFiles(Post entity, Command request)
        {
            if (request.Post.NewFiles?.Any() != true)
            {
                return;
            }

            try
            {
                entity.Files = await _mediaManager.SaveFilesAsync(request.Post.NewFiles);
                entity.IntroText = _mediaManager.GenerateFileLinks(entity.IntroText, entity.Files);
                entity.Text = _mediaManager.GenerateFileLinks(entity.Text, entity.Files);
            }
            catch
            {
                await Cleanup(entity);
                throw;
            }
        }

        private async Task Cleanup(Post post)
        {
            await _searchClient.RemovePostAsync(post);
            _mediaManager.DeleteFiles(post.Files);
            _mediaManager.DeleteFiles(post.Images);
            _mediaManager.DeleteFiles(post.FeaturedImage);
        }
    }
}