namespace SkorinosGimnazija.Application.Authorization;

using Common.Interfaces;
using Dtos;
using FluentValidation;
using MediatR;

public static class UserAuthorize
{
    public record Command(GoogleAuthDto GoogleAuth) : IRequest<UserAuthDto>;

    public class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(v => v.GoogleAuth.Token).NotEmpty();
        }
    }

    public class Handler : IRequestHandler<Command, UserAuthDto>
    {
        private readonly IIdentityService _identityService;

        public Handler(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        public async Task<UserAuthDto> Handle(Command request, CancellationToken _)
        {
            return await _identityService.AuthorizeAsync(request.GoogleAuth.Token);
        }
    }
}