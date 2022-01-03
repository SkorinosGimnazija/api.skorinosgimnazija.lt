namespace SkorinosGimnazija.Infrastructure.Services;

using Application.Common.Identity;
using Application.Common.Interfaces;
using Calendar;
using Google.Apis.Admin.Directory.directory_v1;
using Google.Apis.Admin.Directory.directory_v1.Data;
using Google.Apis.Auth.OAuth2;
using Identity;
using Microsoft.Extensions.Options;
using Options;
using SkorinosGimnazija.Domain.Entities.Identity;

public sealed class EmployeeService : IEmployeeService
{
    private readonly DirectoryService _directoryService;
    private readonly string _domain;
    private readonly Dictionary<string, string[]> _groupRoles = new();

    public EmployeeService(
        IOptions<GoogleOptions> googleOptions,
        IOptions<UrlOptions> urlOptions,
        IOptions<GroupOptions> groupOptions)
    {
        _domain = urlOptions.Value.Domain;

        _groupRoles.Add(groupOptions.Value.Teachers, new[] { Auth.Role.Teacher });
        _groupRoles.Add(groupOptions.Value.BullyManagers, new[] { Auth.Role.BullyManager });
        _groupRoles.Add(groupOptions.Value.Managers, new[] { Auth.Role.Manager, Auth.Role.BullyManager });

        _directoryService = new(new()
        {
            HttpClientInitializer = GoogleCredential.FromJson(googleOptions.Value.Credential)
                .CreateScoped(
                    DirectoryService.ScopeConstants.AdminDirectoryUserReadonly,
                    DirectoryService.ScopeConstants.AdminDirectoryGroupReadonly)
                .CreateWithUser(googleOptions.Value.Admin)
        });
    }

    public async Task<string> GetGroupEmailAsync(string groupId)
    {
        var request = _directoryService.Groups.Get(groupId);
        var response = await request.ExecuteAsync();

        return response.Email;
    }
     
    public async Task<ICollection<string>> GetEmployeeRolesAsync(string userName)
    {
        var userTask = GetEmployeeAsync(userName);
        var groupsTask = GetEmployeeGroupsAsync(userName);
         
        var user = await userTask;
        if (user?.IsAdmin == true)
        {
            return Auth.AllRoles.ToArray();
        }

        var userGroups = await groupsTask;
        var userRoles = new HashSet<string>();

        foreach (var groupId in userGroups)
        {
            if (_groupRoles.TryGetValue(groupId, out var groupRoles))
            {
                userRoles.UnionWith(groupRoles);
            }
        }

        return userRoles;
    }

    public async Task<IEnumerable<TeacherDto>> GetTeachersAsync(CancellationToken ct)
    {
        var teachers = new List<TeacherDto>();
        string? pageToken = null;

        do
        {
            var request = _directoryService.Users.List();

            request.Query = "orgUnitPath=/Teachers isSuspended=false";
            request.Domain = _domain;
            request.PageToken = pageToken;

            var response = await request.ExecuteAsync(ct);

            pageToken = response.NextPageToken;

            teachers.AddRange(response.UsersValue.Select(x => new TeacherDto
            {
                UserName = x.Id,
                DisplayName = x.Name.FullName
            }));
        } while (!string.IsNullOrEmpty(pageToken));

        return teachers;
    }

    public async Task<IEnumerable<string>> GetEmployeeGroupsAsync(string userName)
    {
        try
        {
            var request = _directoryService.Groups.List();

            request.UserKey = userName;
            request.Domain = _domain;

            var response = await request.ExecuteAsync();

            return response.GroupsValue.Select(x => x.Id);
        }
        catch
        {
            return Enumerable.Empty<string>();
        }
    }

    public async Task<Employee?> GetEmployeeAsync(string userName)
    {
        try
        {
            var request = _directoryService.Users.Get(userName);
            var response = await request.ExecuteAsync();

            return new()
            {
                Id = response.Id,
                FullName = response.Name.FullName,
                Email = response.PrimaryEmail,
                IsAdmin = response.IsAdmin == true
            };
        }
        catch
        {
            return null;
        }
    }
}