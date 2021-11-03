﻿namespace API.Dtos;

using System.ComponentModel.DataAnnotations;

public record UserDto
{
    [Required]
    public IEnumerable<string> Roles { get; init; }

    [Required]
    public string UserName { get; init; }
}