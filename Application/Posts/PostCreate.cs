namespace Application.Posts;

using AutoMapper;
using Domain.CMS;
using Dtos;
using Extensions;
using Interfaces;
using MediatR;
using Microsoft.Extensions.Options;
using Persistence;
using System.Diagnostics.CodeAnalysis;
using Utils;

public static class PostCreate
{
    public record Command(PostCreateDto Post) : IRequest<PostDetailsDto>;

    public class Handler : IRequestHandler<Command, PostDetailsDto>
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
        public async Task<PostDetailsDto> Handle(Command request, CancellationToken _)
        {
            await using var transaction = await _context.Database.BeginTransactionAsync();

            var entity = _context.Posts.Add(_mapper.Map<Post>(request.Post)).Entity;
            await _context.SaveChangesAsync();

            await SaveSearchIndex(entity);
            await SaveImages(entity, request);
            await SaveFiles(entity, request);

            await _context.SaveChangesAsync();
            await transaction.CommitAsync();

            return _mapper.Map<PostDetailsDto>(entity);
        }

        private async Task SaveSearchIndex(Post entity)
        {
                await _searchClient.SavePost(_mapper.Map<PostIndexDto>(entity));
        }

        private async Task SaveImages(Post entity, Command request)
        {
            if (request.Post.NewImages is null)
            {
                return;
            }

            try
            {
                entity.Images = await _fileManager.SaveImagesAsync(entity.Id, request.Post.NewImages);
            }
            catch
            {
                _fileManager.DeleteAllFiles(entity.Id);
                await _searchClient.RemovePost(entity.Id);
                throw;
            }
        }

        private async Task SaveFiles(Post entity, Command request)
        {
            if (request.Post.NewFiles is null)
            {
                return;
            }

            try
            {
                entity.Files = await _fileManager.SaveFilesAsync(entity.Id, request.Post.NewFiles);
                entity.GenerateFileLinks(_staticWebUrl);
            }
            catch
            {
                _fileManager.DeleteAllFiles(entity.Id);
                await _searchClient.RemovePost(entity.Id);
                throw;
            }
        }
    }
}