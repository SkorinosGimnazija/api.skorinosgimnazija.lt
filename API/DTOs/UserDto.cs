namespace API.DTOs
{
    using System.Collections.Generic;

    public record UserDto
    {
        public IEnumerable<string> Roles { get; set; }

        public string? UserName { get; set; }
    }
}