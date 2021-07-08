namespace API.DTOs
{
    using System.Collections.Generic;

    public record UserDto
    {
        public string? Email { get; init; }
        
        public IEnumerable<string> Roles { get; init; }

        public string? UserName { get; init; }

        public bool IsAuthenticated { get; init; }
    }
}