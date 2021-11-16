namespace Application.Posts;

using AutoMapper;
using Core;
using Core.Extensions;
using Core.Interfaces;
using Domain.CMS;
using Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Persistence;
using System.Diagnostics.CodeAnalysis;

public static class PostEdit
{
    public record Command(PostEditDto Post) : IRequest<bool>;

    public class Handler : IRequestHandler<Command, bool>
    {
        private readonly DataContext _context;
        private readonly IFileManager _fileManager;
        private readonly IMapper _mapper;
        private readonly ISearchClient _searchClient;
        private readonly string _staticWebUrl;

        public Handler(DataContext context, ISearchClient searchClient, IFileManager fileManager, IMapper mapper,
            IOptions<PublicUrls> urls)
        {
            _context = context;
            _searchClient = searchClient;
            _fileManager = fileManager;
            _mapper = mapper;
            _staticWebUrl = urls.Value.StaticUrl;
        }

        [SuppressMessage("ReSharper", "MethodSupportsCancellation")]
        public async Task<bool> Handle(Command request, CancellationToken _)
        {
            var entity = await _context.Posts.FirstOrDefaultAsync(x => x.Id == request.Post.Id);
            if (entity is null)
            {
                return false;
            }

            _mapper.Map(request.Post, entity);

            await SaveSearchIndex(entity);
            await SaveImages(entity, request);
            await SaveFiles(entity, request);

            await _context.SaveChangesAsync();

            return true;
        }

        private async Task SaveSearchIndex(Post post)
        {
            await _searchClient.SavePost(_mapper.Map<PostIndexDto>(post));
        }

        private async Task SaveImages(Post post, PostEdit.Command request)
        {
            if (post.Images is not null)
            {
                var imagesToDelete = request.Post.Images is null
                    ? post.Images
                    : post.Images
                        .Where(oldImage => request.Post.Images.All(newImage => newImage != oldImage))
                        .ToList();
                 
                if (imagesToDelete.Any())
                {
                    _fileManager.DeleteFiles(imagesToDelete);
                    post.Images = request.Post.Images?.ToList();
                }
            }

            if (request.Post.NewImages is not null)
            {
                var newImages = await _fileManager.SaveImagesAsync(post.Id, request.Post.NewImages);
                post.Images = post.Images?.Concat(newImages).ToList() ?? newImages;
            }
        }

        private async Task SaveFiles(Post post, PostEdit.Command request)
        {
            if (post.Files is not null)
            {
                var filesToDelete = request.Post.Files is null
                    ? post.Files
                    : post.Files
                        .Where(oldFile => request.Post.Files.All(newFile => newFile != oldFile))
                        .ToList();

                if (filesToDelete.Any())
                {
                    _fileManager.DeleteFiles(filesToDelete);
                    post.Files = request.Post.Files?.ToList();
                }
            }

            if (request.Post.NewFiles is not null)
            {
                var newFiles = await _fileManager.SaveFilesAsync(post.Id, request.Post.NewFiles);
                post.Files = post.Files?.Concat(newFiles).ToList() ?? newFiles;
                post.GenerateFileLinks(_staticWebUrl);
            }
        }
    }
}