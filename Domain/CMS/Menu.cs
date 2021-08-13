namespace Domain.CMS
{
    public class Menu
    {
        public int Id { get; init; }

        public int Order { get; set; }

        public string Name { get; set; }

        public string? Slug { get; set; }
        public Domain Domain { get; set; }
        public int DomainId { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }

        public string? Url { get; set; }

        public Menu? ParentMenu { get; set; }
    }
}