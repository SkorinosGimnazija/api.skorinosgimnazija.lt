namespace API.Services.Options;

using System.ComponentModel.DataAnnotations;
using System.Text;
using Google.Apis.Auth.OAuth2;
using JetBrains.Annotations;

public record GoogleOptions
{
    [Required]
    public required string Admin { get; [UsedImplicitly] init; }

    [Required]
    public required string ClientId { get; [UsedImplicitly] init; }

    [Required]
    public required string Credential
    {
        get;
        [UsedImplicitly] init { field = Encoding.UTF8.GetString(Convert.FromBase64String(value)); }
    }

    public GoogleCredential CreateCredential()
    {
        return CredentialFactory.FromJson(Credential,
            JsonCredentialParameters.ServiceAccountCredentialType);
    }
}