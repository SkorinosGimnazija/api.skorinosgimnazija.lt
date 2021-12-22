namespace SkorinosGimnazija.Infrastructure.Services;

using System.Text.Json;
using Application.Common.Identity;
using Application.Common.Interfaces;
using Calendar;
using Google.Apis.Admin.Directory.directory_v1;
using Google.Apis.Auth.OAuth2;
using Microsoft.Extensions.Options;
using Options;

public sealed class GoogleTeachersService : ITeachersService
{
    private readonly DirectoryService _directoryService;
    private readonly string _domain;

    public GoogleTeachersService(
        IOptions<GoogleOptions> googleOptions,
        IOptions<UrlOptions> urlOptions)
    {
        _domain = urlOptions.Value.Domain;
        _directoryService = new(new()
        {
            HttpClientInitializer = GoogleCredential.FromJson(googleOptions.Value.Credential)
                .CreateScoped(DirectoryService.ScopeConstants.AdminDirectoryUserReadonly)
                .CreateWithUser(googleOptions.Value.Admin)
        });
    }

    public async Task<IEnumerable<TeacherDto>> GetTeachersAsync(CancellationToken ct)
    {
        var request = _directoryService.Users.List();

        request.Query = "orgUnitPath=/Teachers isSuspended=false";
        request.Domain = _domain;

        var response = await request.ExecuteAsync(ct);
         
        return response.UsersValue.Select(x => new TeacherDto
        {
            UserName = x.Id,
            DisplayName = x.Name.FullName
        });
    }
}