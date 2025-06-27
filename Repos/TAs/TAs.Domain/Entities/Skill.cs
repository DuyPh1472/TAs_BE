using System.ComponentModel.DataAnnotations;

namespace TAs.Domain.Entities
{
    public class Skill : BaseEntity
    {
        [Key]
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public required string Path { get; set; }
        public required string Icon { get; set; }
        public required string Color { get; set; }
        public required string Description { get; set; }

    }

}