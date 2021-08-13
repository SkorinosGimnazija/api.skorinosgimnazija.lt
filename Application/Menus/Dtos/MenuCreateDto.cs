namespace Domain.CMS
{
    public record MenuCreateDto
    {
        public int Order { get; set; }
        public string Name { get; set; }
        public bool IsPublished { get; set; }
        public string? Slug { get; set; }
        public int DomainId { get; set; }
        public int CategoryId { get; set; }
        public string? Url { get; set; }
        public int? ParentMenuId { get; set; }
    }
}