using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TAs.Domain.Entities
{
    public class Lesson : BaseEntity
    {
        [Key]
        public Guid LessonId { get; set; }
        public Guid CategoryId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Level { get; set; } = string.Empty;
        public string Sentences { get; set; } = string.Empty;
        public string Accent { get; set; } = string.Empty;
        public float Duration { get; set; }
        public string Topics { get; set; } = string.Empty;
        [ForeignKey(nameof(CategoryId))]
        public Category Category { get; set; } = null!;
        public ICollection<Progress> Progresses = [];
        public ICollection<SkillLesson> SkillLessons { get; set; } = [];
        public ICollection<CategoryLesson> CategoryLessons { get; set; } = [];

    }
}