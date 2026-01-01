namespace API.Endpoints.Languages;

using API.Database.Entities.CMS;

public sealed class LanguageMapper : ResponseMapper<LanguageResponse, Language>
{
    public override LanguageResponse FromEntity(Language e)
    {
        return new()
        {
            Id = e.Id,
            Name = e.Name
        };
    }
}