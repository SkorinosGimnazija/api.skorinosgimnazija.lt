namespace SkorinosGimnazija.Application.Common.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities.Identity;

public record UserDto
{
    public int Id { get; init; }
    public string UserName { get; init; } = default!;
    public string DisplayName { get; init; } = default!;
    public string Email { get; init; } = default!;
}
