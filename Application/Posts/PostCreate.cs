namespace Application.Posts
{
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using AutoMapper;
    using Domain.CMS;
    using Dtos;
    using Extensions;
    using Interfaces;
    using MediatR;
    using Microsoft.Extensions.Options;
    using Persistence;
    using Utils;

    public class PostCreate
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
                IOptions<PublicUrls> options)
            {
                _context = context;
                _searchClient = searchClient;
                _fileManager = fileManager;
                _mapper = mapper;
                _staticWebUrl = options.Value.StaticUrl;
            }

            public async Task<PostDetailsDto> Handle(Command request, CancellationToken cancellationToken)
            {
                var entity = _mapper.Map(request.Post, new Post());

                _context.Posts.Add(entity);
                await _context.SaveChangesAsync(cancellationToken);

                if (request.Post.NewImages is not null)
                {
                    entity.Images = await _fileManager.SaveImagesAsync(entity.Id, request.Post.NewImages);
                }

                if (request.Post.NewFiles is not null)
                {
                    entity.Files = await _fileManager.SaveFilesAsync(entity.Id, request.Post.NewFiles);

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

                await _searchClient.SavePost(_mapper.Map(entity, new PostSearchDto()));

                return _mapper.Map(entity, new PostDetailsDto());
            }
            
        }
    }
}