namespace Application.Posts
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
using Application.Utils;
using AutoMapper;
using Dtos;
    using Extensions;
    using Interfaces;
    using MediatR;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Options;
    using Persistence;

    public class PostEdit
    {
        public record Command(PostEditDto Post) : IRequest<bool>;

        public class Handler : IRequestHandler<Command, bool>
        {
            private readonly DataContext _context;
            private readonly IMapper _mapper;
            private readonly ISearchClient _search;
            private readonly IFileManager _fileManager;
            private readonly string _staticWebUrl;

            public Handler(DataContext context, ISearchClient search, IFileManager fileManager,IMapper mapper, IOptions<PublicUrls> options)
            {
                _context = context;
                _search = search;
                _fileManager = fileManager;
                _mapper = mapper;
                _staticWebUrl = options.Value.StaticUrl;
            }


            public async Task<bool> Handle(Command request, CancellationToken cancellationToken)
            {
                var entity = await _context.Posts.FirstOrDefaultAsync(x => x.Id == request.Post.Id, cancellationToken);
                if (entity is null)
                {
                    return false;
                }

                _mapper.Map(request.Post, entity);

                if (entity.Images is not null)
                {
                    var imagesToDelete = request.Post.Images is null
                        ? entity.Images
                        : entity.Images
                            .Where(oldImage => request.Post.Images.All(newImage => newImage != oldImage))
                            .ToList();
                 
                    if (imagesToDelete.Any())
                    {
                        await _fileManager.DeleteFilesAsync(imagesToDelete);
                        entity.Images = request.Post.Images?.ToList();
                    }
                }

                if (request.Post.NewImages is not null)
                {
                    var newImages  = await _fileManager.SaveImagesAsync(entity.Id, request.Post.NewImages);
                    entity.Images = entity.Images?.Concat(newImages).ToList() ?? newImages;
                }

                if (entity.Files is not null)
                {
                    var filesToDelete = request.Post.Files is null
                        ? entity.Files
                        : entity.Files
                            .Where(oldFile => request.Post.Files.All(newFile => newFile != oldFile))
                            .ToList();

                    if (filesToDelete.Any())
                    {
                        await _fileManager.DeleteFilesAsync(filesToDelete);
                        entity.Files = request.Post.Files?.ToList();
                    }
                }

                if (request.Post.NewFiles is not null)
                {
                    var newFiles  = await _fileManager.SaveFilesAsync(entity.Id, request.Post.NewFiles);
                    entity.Files = entity.Files?.Concat(newFiles).ToList() ?? newFiles;

                    if (entity.IntroText is not null)
                    {
                        entity.IntroText = entity.IntroText.GenerateFileLinks(entity.Id, _staticWebUrl);
                    }

                    if (entity.Text is not null)
                    {
                        entity.Text = entity.Text.GenerateFileLinks(entity.Id, _staticWebUrl);
                    }
                }

                await _context.SaveChangesAsync(cancellationToken);

                await _search.UpdatePost(_mapper.Map(entity, new PostSearchDto()));

                return true;
            }

          
        }
    }
}