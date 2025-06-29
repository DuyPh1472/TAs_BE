using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TAs.Domain.Entities
{
    public class SkillLesson : BaseEntity
    {
        [Key]
        public Guid SkillLessonId { get; set; }
        public Guid SkillId { get; set; }
        public Guid LessonId { get; set; }
        [ForeignKey(nameof(SkillId))]
        public Skill Skill { get; set; } = null!;
        [ForeignKey(nameof(LessonId))]
        public Lesson Lesson { get; set; } = null!;
    }
}