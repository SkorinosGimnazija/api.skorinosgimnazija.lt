namespace API.Endpoints.Authorization.Login;

public sealed class LoginEndpoint(IdentityService identityService, AppDbContext dbContext)
    : Endpoint<LoginRequest, AuthorizationResponse>
{
    public override void Configure()
    {
        Post("auth/login");
        AllowAnonymous();
    }

    public override async Task HandleAsync(LoginRequest req, CancellationToken ct)
    {
        var payload = await identityService.ValidateSignatureAsync(req.Token);
        if (payload is null)
        {
            ThrowError(x => x.Token, "Signature not valid");
        }

        var user = await dbContext.Users.AsNoTracking()
                       .FirstOrDefaultAsync(x => x.UserName == payload.Subject, ct);

        if (user is null)
        {
            await Send.ForbiddenAsync(ct);
            return;
        }

        var refreshToken = await identityService.CreateRefreshTokenAsync(user, ct);
        var accessToken = identityService.GenerateAccessToken(user);

        HttpContext.Response.Cookies.Append(
            Auth.Cookie.RefreshTokenName,
            refreshToken.Token,
            Auth.Cookie.RefreshTokenOptions(refreshToken.ExpiresAt));

        await Send.OkAsync(new()
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email,
            Roles = user.Roles,
            Token = accessToken
        }, ct);
    }
}