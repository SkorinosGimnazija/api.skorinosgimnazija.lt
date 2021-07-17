using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Auth
{
    using System.Security.Claims;
    using Application.Interfaces;
    using Microsoft.AspNetCore.Http;
     
    public  class UserAccessor : IUserAccessor
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserAccessor(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string? GetUserName()
        {
         return   _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.Name);
        }
    }
}
