namespace SkorinosGimnazija.Infrastructure.Identity;

using System.Security.Claims;
using Application.Authorization.Dtos;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Calendar;
using Domain.Entities.Identity;
using Domain.Options;
using FluentValidation.Results;
using Google.Apis.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Services;

public sealed class IdentityService : IIdentityService
{
    private readonly IUserClaimsPrincipalFactory<AppUser> _claimsPrincipal;
    private readonly string _domain;
    private readonly IEmployeeService _employeeService;
    private readonly string _googleClientId;
    private readonly TokenService _tokenService;
    private readonly UserManager<AppUser> _userManager;

    public IdentityService(
        TokenService tokenService,
        UserManager<AppUser> userManager,
        IUserClaimsPrincipalFactory<AppUser> claimsPrincipal,
        IEmployeeService employeeService,
        IOptions<GoogleOptions> googleOptions,
        IOptions<UrlOptions> urlOptions)
    {
        _googleClientId = googleOptions.Value.ClientId;
        _domain = urlOptions.Value.Domain;
        _userManager = userManager;
        _tokenService = tokenService;
        _employeeService = employeeService;
        _claimsPrincipal = claimsPrincipal;
    }

    public async Task<AppUser?> GetOrCreateUserAsync(string? userName)
    {
        if (string.IsNullOrEmpty(userName))
        {
            return null;
        }

        var user = await _userManager.FindByNameAsync(userName);
        if (user is not null)
        {
            return user;
        }

        var employee = await _employeeService.GetEmployeeAsync(userName);
        if (employee is null)
        {
            return null;
        }

        return await CreateUserAsync(employee);
    }

    public async Task<UserAuthDto> AuthorizeAsync(string token)
    {
        var payload = await ValidateSignatureAsync(token);
        var user = await _userManager.FindByNameAsync(payload.Subject);

        if (user is null)
        {
            user = await CreateUserAsync(payload);
        }
        else
        {
            await UpdateUserInfoAsync(user, payload);
        }

        await UpdateUserRolesAsync(user);

        var principal = await _claimsPrincipal.CreateAsync(user);

        return new()
        {
            Token = _tokenService.CreateToken(principal.Claims),
            Roles = principal.FindAll(ClaimTypes.Role).Select(x => x.Value),
            DisplayName = user.DisplayName ?? user.Email,
            Email = user.Email
        };
    }

    private async Task UpdateUserRolesAsync(AppUser user)
    {
        var userRoles = await _employeeService.GetEmployeeRolesAsync(user.UserName);
        var currentRoles = await _userManager.GetRolesAsync(user);

        await _userManager.AddToRolesAsync(user, userRoles.Except(currentRoles));
        await _userManager.RemoveFromRolesAsync(user, currentRoles.Except(userRoles));
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
            UserName = payload.Subject,
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

        return user;
    }

    private async Task<AppUser> CreateUserAsync(Employee employee)
    {
        var user = new AppUser
        {
            UserName = employee.Id,
            Email = employee.Email,
            DisplayName = employee.FullName,
            EmailConfirmed = true
        };

        var result = await _userManager.CreateAsync(user);

        if (!result.Succeeded)
        {
            throw new ValidationException(
                result.Errors.Select(x => new ValidationFailure(x.Code, x.Description)));
        }

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