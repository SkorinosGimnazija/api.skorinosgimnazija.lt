namespace SkorinosGimnazija.Application.Posts;

using System.Diagnostics.CodeAnalysis;
using AutoMapper;
using Common.Exceptions;
using Common.Interfaces;
using Domain.Entities;
using Dtos;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Validators;

public static class PostEdit
{
    public record Command(PostEditDto Post) : IRequest<Unit>;

    public class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(x => x.Post).NotNull().SetValidator(new PostEditValidator());
        }
    }

    public class Handler : IRequestHandler<Command, Unit>
    {
        private readonly IAppDbContext _context;
        private readonly IMediaManager _mediaManager;
        private readonly IMapper _mapper;
        private readonly ISearchClient _searchClient;

        public Handler(
            IAppDbContext context, ISearchClient searchClient,
            IMediaManager mediaManager, IMapper mapper)
        {
            _context = context;
            _searchClient = searchClient;
            _mediaManager = mediaManager;
            _mapper = mapper;
        }

        [SuppressMessage("ReSharper", "MethodSupportsCancellation")]
        public async Task<Unit> Handle(Command request, CancellationToken _)
        {
            var entity = await _context.Posts.FirstOrDefaultAsync(x => x.Id == request.Post.Id);
            if (entity is null)
            {
                throw new NotFoundException();
            }

            _mapper.Map(request.Post, entity);

            await SaveSearchIndex(entity);
            await SaveFeaturedImage(entity, request);
            await SaveImages(entity, request);
            await SaveFiles(entity, request);

            await _context.SaveChangesAsync();

            return Unit.Value;
        }

        private async Task SaveSearchIndex(Post post)
        {
            await _searchClient.SavePostAsync(_mapper.Map<PostIndexDto>(post));
        }

     
        private async Task SaveImages(Post post, Command request)
        {
            var imagesToDelete = GetFilesToDelete(post.Images, request.Post.Images);

            if (imagesToDelete?.Any() == true)
            {
                _mediaManager.DeleteFiles(imagesToDelete);
                post.Images = request.Post.Images?.ToList();
            }

            if (request.Post.NewImages?.Any() == true)
            {
                var newImages =
                    await _mediaManager.SaveImagesAsync(request.Post.NewImages, request.Post.OptimizeImages, false);
                post.Images = post.Images?.Concat(newImages).ToList() ?? newImages;
            }
        }

        private async Task SaveFeaturedImage(Post post, Command request)
        {
            if (request.Post.FeaturedImage is null)
            {
                _mediaManager.DeleteFiles(post.FeaturedImage);
            }
            
            if (request.Post.NewFeaturedImage is not null)
            {
                _mediaManager.DeleteFiles(post.FeaturedImage);
                post.FeaturedImage =
                    (await _mediaManager.SaveImagesAsync(new[] { request.Post.NewFeaturedImage },
                         request.Post.OptimizeImages, true))[0];
            }
            else
            {
                post.FeaturedImage = request.Post.FeaturedImage;
            }
        }

        private async Task SaveFiles(Post post, Command request)
        {
            var filesToDelete = GetFilesToDelete(post.Files, request.Post.Files);

            if (filesToDelete?.Any() == true)
            {
                _mediaManager.DeleteFiles(filesToDelete);
                post.Files = request.Post.Files?.ToList();
            }

            if (request.Post.NewFiles?.Any() == true)
            {
                var newFiles = await _mediaManager.SaveFilesAsync(request.Post.NewFiles);
                post.Files = post.Files?.Concat(newFiles).ToList() ?? newFiles;
            }

                post.IntroText = _mediaManager.GenerateFileLinks( post.IntroText, post.Files);
                post.Text = _mediaManager.GenerateFileLinks( post.Text, post.Files);
        }

        private static ICollection<string>? GetFilesToDelete(
            ICollection<string>? currentFiles,
            ICollection<string>? filesToSave)
        {
            if (currentFiles is null || filesToSave?.Any() != true)
            {
                return currentFiles;
            }
             
            return currentFiles.Where(x => !filesToSave.Contains(x)).ToList();
        }

    }
}