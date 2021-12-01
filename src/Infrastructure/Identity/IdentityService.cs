﻿namespace SkorinosGimnazija.Infrastructure.Identity;

using Application.Common.Exceptions;
using Application.Common.Interfaces;
using FluentValidation.Results;
using Google.Apis.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Services;

public sealed class IdentityService : IIdentityService
{
    private const string LoginProvider = "Google";

    private readonly IUserClaimsPrincipalFactory<AppUser> _claimsPrincipal;
    private readonly string _domain;
    private readonly string _googleClientId;
    private readonly TokenService _tokenService;
    private readonly UserManager<AppUser> _userManager;

    public IdentityService(
        UserManager<AppUser> userManager,
        IUserClaimsPrincipalFactory<AppUser> claimsPrincipal,
        TokenService tokenService,
        IConfiguration config)
    {
        _googleClientId = config["Google:ClientId"];
        _domain = config["Urls:Domain"];
        _userManager = userManager;
        _tokenService = tokenService;
        _claimsPrincipal = claimsPrincipal;
    }

    public async Task<string> AuthorizeAsync(string token)
    {
        var payload = await ValidateSignatureAsync(token);
        var user = await _userManager.FindByLoginAsync(LoginProvider, payload.Subject);

        if (user is null)
        {
            user = await CreateUserAsync(payload);
        }
        else
        {
            await UpdateUserInfoAsync(user, payload);
        }

        var principal = await _claimsPrincipal.CreateAsync(user);

        return _tokenService.CreateToken(principal.Claims);
    }

    private async Task UpdateUserInfoAsync(AppUser user, GoogleJsonWebSignature.Payload payload)
    {
        if (string.Equals(user.DisplayName, payload.Name, StringComparison.Ordinal) &&
            string.Equals(user.Email, payload.Email, StringComparison.OrdinalIgnoreCase))
        {
            return;
        }

        user.DisplayName = payload.Name;
        user.Email = payload.Email;

        await _userManager.UpdateAsync(user);
    }

    private async Task<AppUser> CreateUserAsync(GoogleJsonWebSignature.Payload payload)
    {
        var user = new AppUser
        {
            UserName = payload.Email,
            Email = payload.Email,
            DisplayName = payload.Name,
            EmailConfirmed = true
        };

        var result = await _userManager.CreateAsync(user);
        if (!result.Succeeded)
        {
            throw new ValidationException(
                result.Errors.Select(x => new ValidationFailure(x.Code, x.Description)));
        }

        await _userManager.AddToRoleAsync(user, Auth.Role.Teacher);
        await _userManager.AddLoginAsync(user,
            new ExternalLoginInfo(new(), LoginProvider, payload.Subject, LoginProvider));

        return user;
    }

    public async Task<GoogleJsonWebSignature.Payload> ValidateSignatureAsync(string token)
    {
        try
        {
            return await GoogleJsonWebSignature.ValidateAsync(token,
                       new()
                       {
                           HostedDomain = _domain,
                           Audience = new[] { _googleClientId }
                       });
        }
        catch
        {
            throw new UnauthorizedAccessException();
        }
    }
}