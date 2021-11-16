namespace Application.Menus;

using AutoMapper;
using Domain.CMS;
using Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;
using System.Diagnostics.CodeAnalysis;

public static class MenuCreate
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


        [SuppressMessage("ReSharper", "MethodSupportsCancellation")]
        public async Task<MenuDto> Handle(Command request, CancellationToken _)
        {
            var entity = _context.Menus.Add(_mapper.Map<Menu>(request.Menu)).Entity;

                await _context.SaveChangesAsync();

            return _mapper.Map<MenuDto>(entity);
        }
    }
}