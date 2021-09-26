namespace Application.Menus
{
    using System.Threading;
    using System.Threading.Tasks;
    using AutoMapper;
    using Domain.CMS;
    using Dtos;
    using MediatR;
    using Persistence;

    public class MenuCreate
    {
        public record Command(MenuCreateDto Menu) : IRequest<MenuDto>;

        public class Handler : IRequestHandler<Command, MenuDto>
        {
            private readonly DataContext _context;
            private readonly IMapper _mapper;

            public Handler(DataContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<MenuDto> Handle(Command request, CancellationToken cancellationToken)
            {
                var entity = _mapper.Map(request.Menu, new Menu());

                _context.Menus.Add(entity);
                await _context.SaveChangesAsync(cancellationToken);

                return _mapper.Map(entity, new MenuDto());
            }

          
        }
    }
}