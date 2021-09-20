namespace Domain.CMS
{
    public class Menu
    {
        public int Id { get; set; }

        public int Order { get; set; }

        public bool IsPublished { get; set; }

        public string Name { get; set; }

        public string? Slug { get; set; }

        public int CategoryId { get; set; }

        public Category Category { get; set; }

        public string? Url { get; set; }

        public int? ParentMenuId { get; set; }

        public Menu? ParentMenu { get; set; }
    }
}