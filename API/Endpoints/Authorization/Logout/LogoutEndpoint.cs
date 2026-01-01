namespace API.Endpoints.Authorization.Logout;

using API.Database.Entities.Authorization;

public sealed class LogoutEndpoint(IdentityService identityService)
    : EndpointWithoutRequest
{
    public override void Configure()
    {
        Post("auth/logout");
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var token = HttpContext.Request.Cookies[Auth.Cookie.RefreshTokenName];

        if (token is not null && token.Length <= RefreshTokenConfiguration.TokenBase64Length)
        {
            await identityService.DeleteRefreshTokenAsync(token, ct);
        }

        HttpContext.Response.Cookies.Delete(
            Auth.Cookie.RefreshTokenName,
            Auth.Cookie.RefreshTokenOptions(null)
        );

        await Send.NoContentAsync(ct);
    }
}