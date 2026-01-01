namespace API.Endpoints.Authorization.RefreshToken;

using API.Database.Entities.Authorization;

public sealed class RefreshTokenEndpoint(IdentityService identityService)
    : EndpointWithoutRequest<AuthorizationResponse>
{
    public override void Configure()
    {
        Post("auth/refresh-token");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var token = HttpContext.Request.Cookies[Auth.Cookie.RefreshTokenName];
        if (token is null || token.Length > RefreshTokenConfiguration.TokenBase64Length)
        {
            await Send.NoContentAsync(ct);
            return;
        }

        var validToken = await identityService.ValidateRefreshTokenAsync(token, ct);
        if (validToken is null)
        {
            HttpContext.Response.Cookies.Delete(
                Auth.Cookie.RefreshTokenName,
                Auth.Cookie.RefreshTokenOptions(null)
            );
            await Send.NoContentAsync(ct);
            return;
        }

        var newRefreshToken = await identityService.RotateRefreshTokenAsync(validToken, ct);
        var newAccessToken = identityService.GenerateAccessToken(newRefreshToken.User);

        HttpContext.Response.Cookies.Append(
            Auth.Cookie.RefreshTokenName,
            newRefreshToken.Token,
            Auth.Cookie.RefreshTokenOptions(newRefreshToken.ExpiresAt));

        await Send.OkAsync(new()
        {
            Id = newRefreshToken.User.Id,
            Name = newRefreshToken.User.Name,
            Email = newRefreshToken.User.Email,
            Roles = newRefreshToken.User.Roles,
            Token = newAccessToken
        }, ct);
    }
}