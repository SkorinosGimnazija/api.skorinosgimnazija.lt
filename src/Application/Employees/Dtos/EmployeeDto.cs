﻿namespace SkorinosGimnazija.Application.Common.Identity;

public record EmployeeDto
{
    public string UserName { get; init; } = default!;

    public string DisplayName { get; init; } = default!;
}