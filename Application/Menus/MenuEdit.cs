namespace Application.Menus;

using AutoMapper;
using Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;
using System.Diagnostics.CodeAnalysis;

public static class MenuEdit
{
    public record Command(MenuEditDto Menu) : IRequest<bool>;

    public class Handler : IRequestHandler<Command, bool>
    {
        private readonly DataContext _context;

        private readonly IMapper _mapper;

        public Handler(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [SuppressMessage("ReSharper", "MethodSupportsCancellation")]
        public async Task<bool> Handle(Command request, CancellationToken _)
        {
            var entity = await _context.Menus.FirstOrDefaultAsync(x => x.Id == request.Menu.Id);
            if (entity is null)
            {
                return false;
            }

            _mapper.Map(request.Menu, entity);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}