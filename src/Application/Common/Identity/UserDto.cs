﻿namespace SkorinosGimnazija.Application.Common.Identity;

public record UserDto
{
    public int Id { get; init; }

    public string UserName { get; init; } = default!;

    public string DisplayName { get; init; } = default!;

    public string Email { get; init; } = default!;
}