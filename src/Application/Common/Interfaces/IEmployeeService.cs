namespace SkorinosGimnazija.Application.Common.Interfaces;

using SkorinosGimnazija.Application.Common.Identity;
using SkorinosGimnazija.Domain.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public interface IEmployeeService
{
    Task<ICollection<string>> GetEmployeeRolesAsync(string userName);
     
    Task<IEnumerable<Employee>> GetTeachersAsync(CancellationToken ct);
      
    Task<IEnumerable<string>> GetEmployeeGroupsAsync(string userName);

    Task<string> GetGroupEmailAsync(string groupId);
       
    Task<Employee?> GetEmployeeAsync(string userName);

    Task<IEnumerable<Employee>> GetHeadTeachersAsync(CancellationToken ct);
}
