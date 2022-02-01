namespace SkorinosGimnazija.Application.Common.Interfaces;

using Domain.Entities.Identity;

public interface IEmployeeService
{
    Task<ICollection<string>> GetEmployeeRolesAsync(string userName);

    Task<IEnumerable<Employee>> GetTeachersAsync(CancellationToken ct = default);

    Task<IEnumerable<string>> GetEmployeeGroupsAsync(string userName);

    Task<string> GetGroupEmailAsync(string groupId);

    Task<Employee?> GetEmployeeAsync(string userName);

    Task<IEnumerable<Employee>> GetHeadTeachersAsync(CancellationToken ct = default);

    Task<Employee> GetPrincipalAsync(CancellationToken ct = default);
}