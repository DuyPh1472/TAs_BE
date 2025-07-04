namespace TAs.Domain.Entities
{
    public class Skill : BaseEntity
    {
        public required string Name { get; set; }
        public required string Path { get; set; }
        public required string Icon { get; set; }
        public required string Color { get; set; }
        public required string Description { get; set; }
        public ICollection<Category> Categories { get; set; } = [];
    }

}