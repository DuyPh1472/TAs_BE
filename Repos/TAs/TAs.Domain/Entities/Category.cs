using System.ComponentModel.DataAnnotations.Schema;

namespace TAs.Domain.Entities
{
    public class Category : BaseEntity
    {
        public Guid SkillId { get; set; }
        public string CategoryType { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Difficult { get; set; } = string.Empty;
        public string Accent { get; set; } = string.Empty;
        public float Duration { get; set; }
        [ForeignKey(nameof(SkillId))]
        public Skill Skill { get; set; } = null!;
        public ICollection<Lesson> Lessons { get; set; } = new List<Lesson>();
    }
}