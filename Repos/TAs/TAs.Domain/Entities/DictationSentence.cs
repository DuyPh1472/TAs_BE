using System.ComponentModel.DataAnnotations.Schema;

namespace TAs.Domain.Entities
{
    public class DictationSentence : BaseEntity
    {
        public Guid LessonId { get; set; }
        [ForeignKey(nameof(LessonId))]
        public Lesson Lesson { get; set; } = null!;
        public string Text { get; set; } = string.Empty;
        public float StartTime { get; set; } // giây
        public float EndTime { get; set; }   // giây
        public string? AudioUrl { get; set; } // audio riêng cho câu (nếu có)
        public int Position { get; set; }
    }
}