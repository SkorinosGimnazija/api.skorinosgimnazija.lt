﻿namespace SkorinosGimnazija.Domain.Entities;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public sealed class Language
{
    public int Id { get; set; }

    public string Name { get; set; } = default!;

    public string Slug { get; set; } = default!;
}