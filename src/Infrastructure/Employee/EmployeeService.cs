﻿namespace SkorinosGimnazija.Infrastructure.Services;

using Application.Common.Interfaces;
using Calendar;
using Domain.Entities.Identity;
using Domain.Options;
using Google.Apis.Admin.Directory.directory_v1;
using Google.Apis.Auth.OAuth2;
using Identity;
using Microsoft.Extensions.Options;

public sealed class EmployeeService : IEmployeeService
{
    private readonly DirectoryService _directoryService;
    private readonly string _domain;
    private readonly IReadOnlyDictionary<string, IReadOnlyCollection<string>> _groupRoles;

    public EmployeeService(
        IOptions<GoogleOptions> googleOptions,
        IOptions<UrlOptions> urlOptions,
        IOptions<GroupOptions> groupOptions)
    {
        _domain = urlOptions.Value.Domain;

        _groupRoles = new Dictionary<string, IReadOnlyCollection<string>>
        {
            { groupOptions.Value.Teachers, new[] { Auth.Role.Teacher } },
            { groupOptions.Value.BullyManagers, new[] { Auth.Role.BullyManager } },
            { groupOptions.Value.Managers, new[] { Auth.Role.Manager, Auth.Role.BullyManager } },
            { groupOptions.Value.Service, Auth.AllRoles.ToArray() }
        };

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
        var userGroups = await GetEmployeeGroupsAsync(userName);
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

    public Task<IEnumerable<Employee>> GetHeadTeachersAsync(CancellationToken ct = default)
    {
        const string Path = "/Teachers/HeadTeachers";
        return GetEmployesAsync(Path, ct);
    }

    public async Task<Employee> GetPrincipalAsync(CancellationToken ct = default)
    {
        const string Path = "/Teachers/Principal";
        return (await GetEmployesAsync(Path, ct)).First();
    }

    public Task<IEnumerable<Employee>> GetTeachersAsync(CancellationToken ct = default)
    {
        const string Path = "/Teachers";
        return GetEmployesAsync(Path, ct);
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
                Email = response.PrimaryEmail
            };
        }
        catch
        {
            return null;
        }
    }

    private async Task<IEnumerable<Employee>> GetEmployesAsync(string unitPath, CancellationToken ct)
    {
        var employes = new List<Employee>();
        string? pageToken = null;

        do
        {
            var request = _directoryService.Users.List();

            request.Query = $"orgUnitPath={unitPath} isSuspended=false";
            request.Domain = _domain;
            request.PageToken = pageToken;

            var response = await request.ExecuteAsync(ct);

            pageToken = response.NextPageToken;

            employes.AddRange(response.UsersValue.Select(x => new Employee
            {
                Id = x.Id,
                FullName = x.Name.FullName,
                Email = x.PrimaryEmail
            }));
        } while (!string.IsNullOrEmpty(pageToken));

        return employes;
    }
}