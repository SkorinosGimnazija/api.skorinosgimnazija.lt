namespace SkorinosGimnazija.Application.Common.Interfaces;

using SkorinosGimnazija.Application.Common.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public interface ITeachersService
{
    Task<IEnumerable<TeacherDto>> GetTeachersAsync(CancellationToken ct);
}
