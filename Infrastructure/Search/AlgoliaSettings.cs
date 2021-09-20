namespace Infrastructure.Search
{
    public record AlgoliaSettings
    {
        public string AppId { get; set; }

        public string ApiKey { get; set; }
    }
}