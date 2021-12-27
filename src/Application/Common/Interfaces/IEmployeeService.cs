namespace SkorinosGimnazija.Application.Common.Interfaces;

using SkorinosGimnazija.Application.Common.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public interface IEmployeeService
{
    Task<ICollection<string>> GetUserRolesAsync(string userId);

    Task<IEnumerable<TeacherDto>> GetTeachersAsync(CancellationToken ct);

    Task<IEnumerable<string>> GetUserGroupIdsAsync(string userId);

    Task<string> GetGroupEmailAsync(string groupId);
}
