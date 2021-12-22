namespace SkorinosGimnazija.Application.Common.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
 

public record TeacherDto
{
    public string UserName { get; init; } = default!;
    public string DisplayName { get; init; } = default!;
}
