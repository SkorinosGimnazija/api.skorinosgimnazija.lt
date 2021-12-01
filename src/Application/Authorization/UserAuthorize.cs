namespace SkorinosGimnazija.Application.Authorization;

using Common.Interfaces;
using Dtos;
using FluentValidation;
using MediatR;

public static class UserAuthorize
{
    public record Command(AuthDto Auth) : IRequest<AuthDto>;

    public class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(v => v.Auth.Token).NotEmpty();
        }
    }

    public class Handler : IRequestHandler<Command, AuthDto>
    {
        private readonly IIdentityService _identityService;

        public Handler(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        public async Task<AuthDto> Handle(Command request, CancellationToken _)
        {
            var jwtToken = await _identityService.AuthorizeAsync(request.Auth.Token);

            return new() { Token = jwtToken };
        }
    }
}