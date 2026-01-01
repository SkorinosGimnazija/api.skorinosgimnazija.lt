namespace API.Extensions;

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

public static class ClaimsExtensions
{
    extension(ClaimsPrincipal claims)
    {
        public bool IsAdmin()
        {
            return claims.IsInRole(Auth.Role.Admin);
        }

        public bool IsManager()
        {
            return claims.IsInRole(Auth.Role.Manager);
        }

        public bool IsTechManager()
        {
            return claims.IsInRole(Auth.Role.TechManager);
        }

        public bool IsSocialManager()
        {
            return claims.IsInRole(Auth.Role.SocialManager);
        }

        public bool HasId(int? userId)
        {
            if (userId is null)
            {
                return false;
            }

            return claims.HasClaim(JwtRegisteredClaimNames.Sub, userId.Value.ToString());
        }

        public int GetId()
        {
            var idClaim = claims.FindFirstValue(JwtRegisteredClaimNames.Sub);

            if (!int.TryParse(idClaim, out var id))
            {
                return 0;
            }

            return id;
        }
    }
}