﻿namespace SkorinosGimnazija.Application.Accomplishments.Dtos;

public record AccomplishmentClassroomDto
{
    public int Id { get; init; }

    public string Name { get; init; } = default!;
}