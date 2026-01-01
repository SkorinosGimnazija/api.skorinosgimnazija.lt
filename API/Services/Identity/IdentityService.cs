namespace API.Services.Identity;

using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;
using System.Text;
using API.Database.Entities.Authorization;
using API.Services.Employee;
using API.Services.Options;
using FastEndpoints.Security;
using Google.Apis.Auth;
using Microsoft.Extensions.Options;

public sealed class IdentityService(
    IOptions<GoogleOptions> googleOptions,
    IOptions<UrlOptions> urlOptions,
    AppDbContext dbContext)
{
    private static readonly TimeSpan RefreshTokenDuration = TimeSpan.FromDays(180);
    private static readonly TimeSpan AccessTokenDuration = TimeSpan.FromMinutes(30);

    public async Task CreateUserAsync(Employee employee, IEnumerable<string> roles)
    {
        var user = new AppUser { UserName = employee.EmployeeId };
        dbContext.Users.Add(user);

        await UpdateUserAsync(user, employee, roles);
    }

    public async Task UpdateUserAsync(AppUser user, Employee employee, IEnumerable<string> roles)
    {
        user.Name = employee.Name;
        user.Email = employee.Email;
        user.IsSuspended = employee.IsSuspended;
        user.IsTeacher = employee.IsTeacher;
        user.Location = employee.Location;
        user.JobTitle = employee.JobTitle;
        user.Roles = roles.ToList();

        await dbContext.SaveChangesAsync();
    }

    public async Task SuspendUserAsync(AppUser user)
    {
        await DeleteUserRefreshTokensAsync(user.Id);
        user.IsSuspended = true;

        await dbContext.SaveChangesAsync();
    }

    public async Task<RefreshToken> CreateRefreshTokenAsync(
        AppUser user, CancellationToken ct)
    {
        var token = GenerateRefreshToken();
        var tokenHash = GenerateRefreshTokenHash(token);

        var refreshToken = new RefreshToken
        {
            Token = token,
            TokenHash = tokenHash,
            UserId = user.Id,
            ExpiresAt = DateTime.UtcNow.Add(RefreshTokenDuration)
        };

        dbContext.RefreshTokens.Add(refreshToken);
        await dbContext.SaveChangesAsync(ct);

        return refreshToken;
    }

    public async Task<GoogleJsonWebSignature.Payload?> ValidateSignatureAsync(string token)
    {
        try
        {
            return await GoogleJsonWebSignature.ValidateAsync(token, new()
            {
                HostedDomain = urlOptions.Value.Domain,
                Audience = [googleOptions.Value.ClientId]
            });
        }
        catch
        {
            return null;
        }
    }

    public async Task<RefreshToken?> ValidateRefreshTokenAsync(
        string token, CancellationToken ct)
    {
        var tokenHash = GenerateRefreshTokenHash(token);
        var refreshToken = await dbContext.RefreshTokens
                               .Include(x => x.User)
                               .Where(x =>
                                   x.TokenHash == tokenHash &&
                                   x.ExpiresAt >= DateTime.UtcNow)
                               .FirstOrDefaultAsync(ct);

        return refreshToken;
    }

    public async Task<RefreshToken> RotateRefreshTokenAsync(
        RefreshToken refreshToken, CancellationToken ct)
    {
        refreshToken.Token = GenerateRefreshToken();
        refreshToken.TokenHash = GenerateRefreshTokenHash(refreshToken.Token);
        refreshToken.ExpiresAt = DateTime.UtcNow.Add(RefreshTokenDuration);

        await dbContext.SaveChangesAsync(ct);

        return refreshToken;
    }

    public async Task DeleteRefreshTokenAsync(string token, CancellationToken ct)
    {
        var tokenHash = GenerateRefreshTokenHash(token);
        var refreshToken = await dbContext.RefreshTokens
                               .FirstOrDefaultAsync(x => x.TokenHash == tokenHash, ct);

        if (refreshToken is not null)
        {
            dbContext.RefreshTokens.Remove(refreshToken);
            await dbContext.SaveChangesAsync(ct);
        }
    }

    public async Task DeleteUserRefreshTokensAsync(int userId)
    {
        await dbContext.RefreshTokens.Where(x => x.UserId == userId).ExecuteDeleteAsync();
    }

    public string GenerateAccessToken(AppUser user)
    {
        return JwtBearer.CreateToken(x =>
        {
            x.User.Roles.AddRange(user.Roles);
            x.User.Claims.Add((JwtRegisteredClaimNames.Sub, user.Id.ToString()));
            x.ExpireAt = DateTime.UtcNow.Add(AccessTokenDuration);
        });
    }

    private string GenerateRefreshToken()
    {
        return Convert.ToBase64String(
            RandomNumberGenerator.GetBytes(RefreshTokenConfiguration.TokenHashLength));
    }

    private byte[] GenerateRefreshTokenHash(string token)
    {
        return SHA256.HashData(Encoding.UTF8.GetBytes(token));
    }
}