namespace API.Controllers
{
    using System;
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using DTOs;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;

    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly ILogger<AuthController> _logger;

        private readonly SignInManager<IdentityUser> _signInManager;

        private readonly UserManager<IdentityUser> _userManager;

        public AuthController(
            ILogger<AuthController> logger,
            SignInManager<IdentityUser> signinMgr,
            UserManager<IdentityUser> userMgr)
        {
            _logger = logger;
            _signInManager = signinMgr;
            _userManager = userMgr;
        }

        [HttpGet("user")]
        public ActionResult<UserDto> GetUser()
        {
            if (User.Identity?.IsAuthenticated != true)
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

        [HttpGet("login")]
        public IActionResult Login(string? returnUrl = null)
        {
            var redirectUrl = Url.Action(nameof(LoginResponse), new { ReturnUrl = returnUrl });
            var properties = _signInManager.ConfigureExternalAuthenticationProperties("Google", redirectUrl);

            return Challenge(properties, "Google");
        }

        [HttpGet("login-response")]
        public async Task<IActionResult> LoginResponse(string? returnUrl = null)
        {
            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                return RedirectToAction(nameof(Login));
            }

            returnUrl = returnUrl != null ? Uri.UnescapeDataString(returnUrl) : Url.Content("~/");

            var loginResult = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, false);
            if (loginResult.Succeeded)
            {
                return Redirect(returnUrl);
            }

            var user = new IdentityUser
            {
                Email = info.Principal.FindFirst(ClaimTypes.Email)!.Value,
                UserName = info.Principal.FindFirst(ClaimTypes.Name)!.Value,
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
                _logger.LogError($"User login failed // {user}");
                return BadRequest(result.Errors);
            }

            await _signInManager.SignInAsync(user, false);
            _logger.LogInformation($"User created // {user}");

            return Redirect(returnUrl);
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return Ok();
        }
    }
}