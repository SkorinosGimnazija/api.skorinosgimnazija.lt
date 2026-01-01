namespace API.Services.Employee;

using API.Services.Options;
using Google.Apis.Admin.Directory.directory_v1;
using Microsoft.Extensions.Options;

public sealed class EmployeeService(
    IOptions<GoogleOptions> googleOptions,
    IOptions<UrlOptions> urlOptions,
    IOptions<GroupOptions> groupOptions)
{
    private readonly DirectoryService _directoryService = new(new()
    {
        HttpClientInitializer = googleOptions.Value.CreateCredential()
            .CreateScoped(
                DirectoryService.ScopeConstants.AdminDirectoryUserReadonly,
                DirectoryService.ScopeConstants.AdminDirectoryGroupReadonly)
            .CreateWithUser(googleOptions.Value.Admin)
    });

    private readonly IReadOnlyDictionary<string, IReadOnlyCollection<string>> _groupRoles =
        new Dictionary<string, IReadOnlyCollection<string>>
        {
            {
                groupOptions.Value.Teachers,
                [Auth.Role.Teacher]
            },
            {
                groupOptions.Value.SocialManagers,
                [Auth.Role.SocialManager]
            },
            {
                groupOptions.Value.TechManagers,
                [Auth.Role.TechManager]
            },
            {
                groupOptions.Value.Managers,
                [Auth.Role.Manager, Auth.Role.SocialManager, Auth.Role.Teacher]
            },
            {
                groupOptions.Value.Service,
                Auth.Role.All
            }
        };

    public async Task<IReadOnlyDictionary<string, HashSet<string>>> GetEmployeeGroupsAsync()
    {
        var employeeRoles = new Dictionary<string, HashSet<string>>();

        var groupTasks = _groupRoles.Select(async group =>
        {
            var employeeIds = await GetEmployeeIdsByGroupAsync(group.Key);
            var roles = group.Value;

            return (roles, employeeIds);
        });

        foreach (var (roles, employeeIds) in await Task.WhenAll(groupTasks))
        {
            foreach (var employeeId in employeeIds)
            {
                if (!employeeRoles.TryGetValue(employeeId, out var currentRoles))
                {
                    employeeRoles[employeeId] = currentRoles = [];
                }

                currentRoles.UnionWith(roles);
            }
        }

        return employeeRoles;
    }

    public async Task<List<string>> GetEmployeeIdsByGroupAsync(string groupId)
    {
        var employeeIds = new List<string>();
        var pageToken = string.Empty;

        do
        {
            var request = _directoryService.Members.List(groupId);
            request.PageToken = pageToken;

            var response = await request.ExecuteAsync();
            pageToken = response.NextPageToken;

            employeeIds.AddRange(response.MembersValue.Select(x => x.Id));
        } while (!string.IsNullOrEmpty(pageToken));

        return employeeIds;
    }

    public async Task<List<Employee>> GetEmployeesAsync()
    {
        var employees = new List<Employee>();
        var pageToken = string.Empty;

        do
        {
            var request = _directoryService.Users.List();

            request.Domain = urlOptions.Value.Domain;
            request.PageToken = pageToken;
            request.Projection = UsersResource.ListRequest.ProjectionEnum.Full;

            var response = await request.ExecuteAsync();
            pageToken = response.NextPageToken;

            employees.AddRange(response.UsersValue.Select(x => new Employee
            {
                EmployeeId = x.Id,
                Name = x.Name.FullName,
                Email = x.PrimaryEmail,
                UnitPath = x.OrgUnitPath,
                IsSuspended = x.Suspended == true,
                JobTitle = x.Organizations?.FirstOrDefault()?.Title,
                Location = x.Organizations?.FirstOrDefault()?.Description
            }));
        } while (!string.IsNullOrEmpty(pageToken));

        return employees;
    }
}