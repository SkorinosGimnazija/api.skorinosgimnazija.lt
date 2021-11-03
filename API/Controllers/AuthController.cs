namespace API.Controllers;

using System.Net.Mime;
using System.Security.Claims;
using Domain.Auth;
using Dtos;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

[Route("auth")]
[ApiController]
[Produces(MediaTypeNames.Application.Json)]
public class AuthController : ControllerBase
{
    private readonly ILogger<AuthController> _logger;

    private readonly SignInManager<AppUser> _signInManager;

    private readonly UserManager<AppUser> _userManager;

    public AuthController(ILogger<AuthController> logger, SignInManager<AppUser> signinMgr,
        UserManager<AppUser> userMgr)
    {
        _logger = logger;
        _signInManager = signinMgr;
        _userManager = userMgr;
    }

    [HttpGet("user", Name = "GetUser")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public ActionResult<UserDto> GetUser()
    {
        if (!_signInManager.IsSignedIn(User))
        {
            return NoContent();
        }

        return Ok(
            new UserDto
            {
                UserName = User.FindFirstValue(ClaimTypes.Name),
                Roles = User.FindAll(ClaimTypes.Role).Select(x => x.Value)
            });
    }

    [HttpGet("login-google")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public IActionResult LoginGoogle(string returnUrl)
    {
        var redirectUrl = Url.Action(nameof(LoginCallback), new { returnUrl });
        var properties = _signInManager.ConfigureExternalAuthenticationProperties(
            GoogleDefaults.AuthenticationScheme,
            redirectUrl);

        return Challenge(properties, GoogleDefaults.AuthenticationScheme);
    }

    [HttpGet("login-callback")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public async Task<IActionResult> LoginCallback(string returnUrl)
    {
        var info = await _signInManager.GetExternalLoginInfoAsync();
        if (info == null)
        {
            return BadRequest();
        }

        var loginResult =
            await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, false);
        if (loginResult.Succeeded)
        {
            return Redirect(Uri.UnescapeDataString(returnUrl));
        }

        var user = new AppUser
        {
            Email = info.Principal.FindFirstValue(ClaimTypes.Email),
            UserName = info.Principal.FindFirstValue(ClaimTypes.Name)
        };

        var result = await _userManager.CreateAsync(user);
        if (!result.Succeeded)
        {
            _logger.LogError($"User creation failed // {user}");
            return BadRequest(result.Errors);
        }

        result = await _userManager.AddLoginAsync(user, info);
        if (!result.Succeeded)
        {
            _logger.LogError($"User login info failed // {user}");
            return BadRequest(result.Errors);
        }

        await _signInManager.SignInAsync(user, false);

        return Redirect(Uri.UnescapeDataString(returnUrl));
    }

    [HttpPost("logout", Name = "Logout")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return NoContent();
    }
}