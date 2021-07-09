namespace API.DTOs
{
    using System.Collections.Generic;

    public record UserDto
    {
        public IEnumerable<string> Roles { get; init; }

        public string? UserName { get; init; }
    }
}