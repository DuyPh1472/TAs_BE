using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TAs.Domain.Entities
{
    public class CategoryLesson : BaseEntity
    {
        [Key]
        public Guid CategoryLessonId { get; set; }
        public Guid LessonId { get; set; }
        public Guid CategoryId { get; set; }
        [ForeignKey(nameof(LessonId))]
        public Lesson Lesson { get; set; } = null!;
        [ForeignKey(nameof(CategoryId))]
        public Category Category { get; set; } = null!;
    }
}