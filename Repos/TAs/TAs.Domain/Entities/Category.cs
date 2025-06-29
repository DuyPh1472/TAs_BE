using System.ComponentModel.DataAnnotations;

namespace TAs.Domain.Entities
{
    public class Category : BaseEntity
    {
        [Key]
        public Guid CategoryId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Difficult { get; set; } = string.Empty;
        public string Accent { get; set; } = string.Empty;
        public float Duration { get; set; }
        public ICollection<CategoryLesson> CategoryLessons { get; set; } = [];
    }
}