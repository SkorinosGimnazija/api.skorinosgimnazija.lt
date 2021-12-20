namespace SkorinosGimnazija.Application.Common.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public record AppUserDto
{
    public int Id { get; init; }
    public string DisplayName { get; init; } = default!;
    public string Email { get; init; } = default!;
     
}
